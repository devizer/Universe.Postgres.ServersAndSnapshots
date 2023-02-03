using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Universe
{
    public static class ExecProcessHelper
    {
        public class ExecResult
        {
            public int ExitCode { get; set; }
            public string ErrorText { get; set; }
            public string OutputText { get; set; }
            public Exception OutputException { get; set; }
            public Exception ErrorException { get; set; }
            public bool IsTimeout { get; set; }
            public int MillisecondsTimeout { get; set; }

            public void DemandGenericSuccess(string operationDescription, bool failOnStderr = false)
            {
                bool isFail =
                    IsTimeout
                    || ExitCode != 0
                    || ErrorException != null
                    || OutputException != null
                    || failOnStderr && !string.IsNullOrEmpty(ErrorText);

                if (isFail)
                {
                    StringBuilder reason = new StringBuilder();
                    if (IsTimeout) reason.AppendLine($"  - Operation canceled by timeout ({(MillisecondsTimeout > 0 ? $"{MillisecondsTimeout:n0} milliseconds" : "infinite")})");
                    if (ExitCode != 0) reason.AppendLine($"  - Exit code is {ExitCode}");
                    if (!string.IsNullOrEmpty(ErrorText)) reason.AppendLine($"  - Std Error: {ErrorText}");
                    if (OutputException != null) reason.AppendLine($"  - Output stream exception {OutputException}");
                    if (ErrorException != null) reason.AppendLine($"  - Error stream exception {ErrorException}");

                    throw new ProcessInvocationException(
                        $"{operationDescription} failed. The reason is:{Environment.NewLine}{reason}",
                        ExitCode,
                        ErrorText);
                }
            }
        }

        public static ExecResult HiddenExec(string command, string args, int millisecondsTimeout = -1, IDictionary<string,string> environment = null, string standardInputText = null)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(command, args)
            {
                // CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false,
            };

            if (standardInputText != null)
            {
                startInfo.RedirectStandardInput = true;
#if !NETFRAMEWORK && !NETCOREAPP2_0 && !NETCOREAPP1_1                
                // is not used
                startInfo.StandardInputEncoding = Encoding.UTF8;
#endif
            }

#if NETFRAMEWORK
            var envDictionary = startInfo.EnvironmentVariables;
#else
            var envDictionary = startInfo.Environment;
#endif
            envDictionary["LANG"] = "C";
            envDictionary["LC_ALL"] = "C";
            if (environment != null)
                foreach (var pair in environment)
                    envDictionary[pair.Key] = pair.Value;

            Process process = new Process()
            {
                StartInfo = startInfo,
            };

            object syncOutput = new object(), syncError = new object();
            string ReadLineByLine(StreamReader source)
            {
                StringBuilder retString = new StringBuilder();
                while (true)
                {
                    var line = source.ReadLine();
                    if (line == null) break;
                    retString.AppendLine(line);
                }

                return retString.ToString();
            }

            ManualResetEventSlim outputDone = new ManualResetEventSlim(false);
            ManualResetEventSlim errorDone = new ManualResetEventSlim(false);
            string outputText = null;
            StringBuilder outputTextBuilder = new StringBuilder();
            string errorText = null;
            StringBuilder errorTextBuilder = new StringBuilder();
            Exception outputException = null;
            Exception errorException = null;

            using (process)
            {
                process.Start();
                if (standardInputText != null)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        StringReader rdr = new StringReader(standardInputText);
                        string inputLine;
                        using (StreamWriter inputWriter = process.StandardInput)
                            while ((inputLine = rdr.ReadLine()) != null)
                                inputWriter.WriteLine(inputLine);
                        /*
                        using (StreamWriter inputWriter = process.StandardInput)
                        {
                            inputWriter.WriteLine(standardInputText);
                        }
                        */
                    });
                }

                // void 

                ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            var buffer = ReadLineByLine(process.StandardError);
                            lock (syncError) errorTextBuilder.Append(buffer);
                            // errorText = errorTextBuilder.ToString();
                            // errorText = process.StandardError.ReadToEnd();
                        }
                        catch (Exception ex)
                        {
                            errorException = ex;
                        }
                        finally
                        {
                            errorDone.Set();
                        }
                    }
                );

                ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            var buffer = ReadLineByLine(process.StandardOutput);
                            lock (syncOutput) outputTextBuilder.Append(buffer);
                        }
                        catch (Exception ex)
                        {
                            outputException = ex;
                        }
                        finally
                        {
                            outputDone.Set();
                        }
                    }
                );

                Stopwatch startAt = Stopwatch.StartNew();
                bool isStreamFinished = WaitHandle.WaitAll(
                    new[] { errorDone.WaitHandle, outputDone.WaitHandle },
                    millisecondsTimeout);

                int remainingMilliseconds =
                    isStreamFinished
                    ? Math.Max(1, millisecondsTimeout - (int)startAt.ElapsedMilliseconds)
                    : 1;

                if (millisecondsTimeout < 0) remainingMilliseconds = -1;

                bool isProcessFinished = isStreamFinished || process.WaitForExit(remainingMilliseconds);

                // if (args.EndsWith("start", StringComparison.CurrentCultureIgnoreCase)) if (Debugger.IsAttached) Debugger.Break();

                bool isSuccess = isProcessFinished;

                var exitCode = isSuccess ? process.ExitCode : -1;

                // lock(syncError)
                errorText = errorTextBuilder.ToString();

                // System.ArgumentOutOfRangeException : Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'chunkLength')
                // lock (syncOutput)
                outputText = outputTextBuilder.ToString();

                errorText = errorText?.TrimEnd('\r', '\n');
                return new ExecResult()
                {
                    IsTimeout = !isSuccess,
                    ExitCode = exitCode,
                    OutputText = outputText,
                    ErrorText = errorText,
                    OutputException = outputException,
                    ErrorException = errorException,
                    MillisecondsTimeout = millisecondsTimeout,
                };
            }
        }

        /*
        private static void ReadLineByLine(StringBuilder result, StreamReader source)
        {
            while (true)
            {
                var line = source.ReadLine();
                if (line == null) break;
                result.AppendLine(line);
            }
        }
    */
    }

    public class ProcessInvocationException : Exception
    {
        public int ExitCode { get; set; }
        public string ErrorText { get; set; }

        public ProcessInvocationException(string message, int exitCode, string errorText) : base(message)
        {
            ExitCode = exitCode;
            ErrorText = errorText;
        }
    }
}