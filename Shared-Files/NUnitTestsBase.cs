using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Universe.CpuUsage;
using Universe.Postgres.ServersAndSnapshots;

namespace Universe.NUnitTests
{
    public class NUnitTestsBase
    {
        public static bool IsTravis => Environment.GetEnvironmentVariable("TRAVIS") == "true";

        protected static TextWriter OUT;
        private Stopwatch StartAt;
        private CpuUsage.CpuUsage? _CpuUsage_OnStart;
        private int TestCounter = 0, TestClassCounter = 0;
        private int TestCounterStorage = 0;
        private static int TestClassCounterStorage = 0;


        Action OnDisposeList = () => { };

        private int OnDisposeCounter = 0;
        protected void OnDispose(string title, Action action)
        {
            OnDisposeList += () =>
            {
                try
                {
                    action();
                    Console.WriteLine($"[On Dispose Info {TestId}] {title} success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[On Dispose Error {TestId}] {title} failed.{Environment.NewLine}{ex}");
                }
            };
        }

        private string TestId => $"#{TestClassCounter}.{TestCounter}";

        protected void OnDispose(Action action)
        {
            OnDispose($"Dispose Action {TestId}.{Interlocked.Increment(ref OnDisposeCounter)}", action);
        }

        protected void OnDisposeSilent(string actionTitle, Action action)
        {
            OnDispose(actionTitle, () => TryAndForget.Execute(action));
        }

        protected void OnDisposeSilent(Action action)
        {
            OnDispose($"Dispose Action {TestId}.{Interlocked.Increment(ref OnDisposeCounter)}", () => TryAndForget.Execute(action));
        }


        [SetUp]
        public void BaseSetUp()
        {
            TestConsole.Setup();
            Environment.SetEnvironmentVariable("SKIP_FLUSHING", null);
            StartAt = Stopwatch.StartNew();
            _CpuUsage_OnStart = GetCpuUsage();
            TestCounter = Interlocked.Increment(ref TestCounterStorage);

            var testClassName = TestContext.CurrentContext.Test.ClassName;
            testClassName = testClassName.Split('.').LastOrDefault();
            Console.WriteLine($"#{TestClassCounter}.{TestCounter} {{{TestContext.CurrentContext.Test.Name}}} @ {testClassName} starting...");
        }

        private CpuUsage.CpuUsage? GetCpuUsage()
        {
            try
            {
                // return LinuxResourceUsage.GetByThread();
                return CpuUsage.CpuUsage.Get(CpuUsageScope.Thread);
            }
            catch
            {
            }

            return null;
        }

        [TearDown]
        public void BaseTearDown()
        {
            var elapsed = StartAt.Elapsed;
            var cpuUsage = "";
            if (_CpuUsage_OnStart.HasValue)
            {
                var onEnd = GetCpuUsage();
                if (onEnd != null)
                {
                    var delta = CpuUsage.CpuUsage.Substruct(onEnd.Value, _CpuUsage_OnStart.Value);
                    var user = delta.UserUsage.TotalMicroSeconds / 1000d;
                    var kernel = delta.KernelUsage.TotalMicroSeconds / 1000d;
                    var perCents = (user + kernel) / 1000d / elapsed.TotalSeconds;
                    cpuUsage = $" (cpu: {perCents * 100:f0}%, {user + kernel:n3} = {user:n3} [user] + {kernel:n3} [kernel] milliseconds)";
                }
            }

            Console.WriteLine(
                $"#{TestClassCounter}.{TestCounter} {{{TestContext.CurrentContext.Test.Name}}} >{TestContext.CurrentContext.Result.Outcome.Status.ToString().ToUpper()}< in {elapsed}{cpuUsage}");

            var copy = OnDisposeList;
            OnDisposeList = () => { };
            copy();

            Console.WriteLine("");
        }

        [OneTimeSetUp]
        public void BaseOneTimeSetUp()
        {
            TestClassCounter = Interlocked.Increment(ref TestClassCounterStorage);
            TestConsole.Setup();
        }

        [OneTimeTearDown]
        public void BaseOneTimeTearDown()
        {
            // nothing todo
        }

        protected static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
				return false;
#endif
            }
        }

        public class TestConsole
        {
            private static bool Done = false;

            public static void Setup()
            {
                if (!Done)
                {
                    Done = true;
                    Console.SetOut(new TW());
                }
            }

            private class TW : TextWriter
            {
                public override Encoding Encoding { get; }

                public override void WriteLine(string value)
                {
                    //                    TestContext.Progress.Write(string.Join(",", value.Select(x => ((int)x).ToString("X2"))) );
                    //                    if (value.Length > Environment.NewLine.Length && value.EndsWith(Environment.NewLine))
                    //                        value = value.Substring(0, value.Length - Environment.NewLine.Length);


                    try
                    {
                        TestContext.Progress.WriteLine(value);
                        // TestContext.Error.WriteLine(value); // .WriteLine();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RequiredWindowsAttribute : NUnitAttribute, IApplyToTest
    {
        public void ApplyToTest(Test test)
        {
            if (test.RunState == RunState.NotRunnable)
            {
                return;
            }

            if (CrossInfo.ThePlatform != CrossInfo.Platform.Windows)
            {
                test.RunState = RunState.Ignored;
                test.Properties.Set(PropertyNames.SkipReason, "This test should run only on Windows");
            }
        }
    }
}
