using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Universe.NUnitTests;
using static NUnit.Framework.Assert;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    
    public class WindowsProcessListTest : NUnitTestsBase
    {
        [Test/*, Explicit*/]
        [TestCase("First")]
        [TestCase("Next")]
        public void TestGetAllProcesses(string id)
        {
            if (!TinyCrossInfo.IsWindows) return;

            Stopwatch startGetAt = Stopwatch.StartNew();
            WindowsProcessInterop.PROCESSENTRY32[] processes = WindowsProcessInterop.GetAllProcesses();
            var msecGet = startGetAt.ElapsedTicks * 1000d / Stopwatch.Frequency;
            foreach (var process in processes)
            {
                Console.WriteLine(process);
            }

            Console.WriteLine($"WindowsProcessInterop.GetAllProcesses() returned {processes.Length} and took {msecGet:n2} milliseconds");
            Stopwatch startAsDictionaryAt = Stopwatch.StartNew();
            var dictionary = processes.AsParentDictionary();
            var msecAsDictionary = startAsDictionaryAt.ElapsedTicks * 1000d / Stopwatch.Frequency;
            Console.WriteLine($"WindowsProcessInteropExtensions.AsChildrenDictionary() took {msecAsDictionary:n2} milliseconds");

            var tempPid = Process.GetCurrentProcess().Id;
            var current = processes.FirstOrDefault(x => x.ProcessID == tempPid);
            Console.WriteLine($"Current Process: {current}");
            dictionary.TryGetValue(current.ParentProcessID, out var parentId);
            Console.WriteLine($"Parent Process ID: {parentId}");
            var parent = processes.FirstOrDefault(x => x.ProcessID == parentId);
            Console.WriteLine($"Parent Process: {parent}");

            // Deep Children
            var servicesProcess = processes.FirstOrDefault(x => x.ExeFile?.Equals("services.exe", StringComparison.OrdinalIgnoreCase) ?? false);
            Console.WriteLine($"services.exe process: {servicesProcess}");

            Stopwatch startAsChildrenDictionaryAt = Stopwatch.StartNew();
            var deepChildren = processes.AsChildrenDictionary().GetDeepChildren(servicesProcess.ProcessID).ToArray();
            var msecAsChildrenDictionaryAt = startAsDictionaryAt.ElapsedTicks * 1000d / Stopwatch.Frequency;
            Console.WriteLine($"WindowsProcessInteropExtensions.AsChildrenDictionary().GetDeepChildren({servicesProcess.ProcessID}) took {startAsChildrenDictionaryAt:n2} milliseconds returned {deepChildren.Count()}: {string.Join(",", deepChildren)}");

            Assert.AreEqual(tempPid, current.ProcessID, @"Current PID not found");
            Assert.AreNotEqual(0, current.ProcessID, @"Current PID is not zero");
            Assert.AreNotEqual(0, parentId, @"Parent PID is not zero");
            Assert.AreNotEqual(0, parent.ProcessID, $@"Parent Process {parentId} not found");

        }
    }
}
