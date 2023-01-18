using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Universe.Postgres.ServersAndSnapshots.WindowsProcessInterop;

namespace Universe.Postgres.ServersAndSnapshots
{

    public static class WindowsProcessInterop
    {

        public static PROCESSENTRY32[] GetAllProcesses()
        {
            List<PROCESSENTRY32> ret = new List<PROCESSENTRY32>();
            IntPtr handleToSnapshot = IntPtr.Zero;
            try
            {
                PROCESSENTRY32 procEntry = new PROCESSENTRY32();
                procEntry.dwSize = (UInt32)Marshal.SizeOf(typeof(PROCESSENTRY32));
                handleToSnapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);
                if (Process32First(handleToSnapshot, ref procEntry))
                {
                    do
                    {
                        var copy = procEntry;
                        ret.Add(copy);
                    } while (Process32Next(handleToSnapshot, ref procEntry));
                }
                else
                {
                    throw new Exception(string.Format("Failed with win32 error code {0}", Marshal.GetLastWin32Error()));
                }
            }
            finally
            {
                // Must clean up the snapshot object!
                CloseHandle(handleToSnapshot);
            }

            return ret.ToArray();
        }


        //inner enum used only internally
        [Flags]
        private enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            Inherit = 0x80000000,
            All = 0x0000001F,
            NoHeaps = 0x40000000
        }

        //inner struct used only internally
        [StructLayout(LayoutKind.Sequential /*, CharSet = CharSet.Auto */)]
        public struct PROCESSENTRY32
        {
            const int MAX_PATH = 260;
            internal UInt32 dwSize;
            internal UInt32 cntUsage;
            public UInt32 ProcessID;
            internal IntPtr th32DefaultHeapID;
            internal UInt32 th32ModuleID;
            internal UInt32 cntThreads;
            public UInt32 ParentProcessID;
            internal Int32 pcPriClassBase;
            internal UInt32 dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string ExeFile;

            public override string ToString()
            {
                return $"PID: {ProcessID}, Parent: {ParentProcessID}, Exe: '{ExeFile}'";
            }
        }

        [DllImport("kernel32", SetLastError = true /*, CharSet = CharSet.Auto */)]
        static extern IntPtr CreateToolhelp32Snapshot([In] UInt32 dwFlags, [In] UInt32 th32ProcessID);

        [DllImport("kernel32", SetLastError = true /*, CharSet = CharSet.Auto */)]
        static extern bool Process32First([In] IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32", SetLastError = true /*, CharSet = CharSet.Auto */)]
        static extern bool Process32Next([In] IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle([In] IntPtr hObject);

        // get the parent process given a pid
        public static Process GetParentProcess(int pid)
        {
            Process parentProc = null;
            IntPtr handleToSnapshot = IntPtr.Zero;
            try
            {
                PROCESSENTRY32 procEntry = new PROCESSENTRY32();
                procEntry.dwSize = (UInt32)Marshal.SizeOf(typeof(PROCESSENTRY32));
                handleToSnapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);
                if (Process32First(handleToSnapshot, ref procEntry))
                {
                    do
                    {
                        if (pid == procEntry.ProcessID)
                        {
                            parentProc = Process.GetProcessById((int)procEntry.ParentProcessID);
                            break;
                        }
                    } while (Process32Next(handleToSnapshot, ref procEntry));
                }
                else
                {
                    throw new Exception(string.Format("Failed with win32 error code {0}", Marshal.GetLastWin32Error()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't get the process.", ex);
            }
            finally
            {
                // Must clean up the snapshot object!
                CloseHandle(handleToSnapshot);
            }

            return parentProc;
        }

        // get the specific parent process
        public static Process CurrentParentProcess
        {
            get { return GetParentProcess(Process.GetCurrentProcess().Id); }
        }
/*
        static void Main()
        {
            Process pr = CurrentParentProcess;

            Console.WriteLine("Parent Proc. ID: {0}, Parent Proc. name: {1}", pr.Id, pr.ProcessName);
        }
*/
    }

    public static class WindowsProcessInteropExtensions
    {
        public static IDictionary<uint, uint> AsParentDictionary(this PROCESSENTRY32[] processes)
        {
            Dictionary<uint, uint> ret = new Dictionary<uint, uint>();
            foreach (var process in processes)
                ret[process.ProcessID] = process.ParentProcessID;

            return ret;
        }
        public static IDictionary<uint, List<uint>> AsChildrenDictionary(this PROCESSENTRY32[] processes)
        {
            Dictionary<uint, List<uint>> ret = new Dictionary<uint, List<uint>>();
            foreach (var process in processes)
            {
                var idParent = process.ParentProcessID;
                if (!ret.TryGetValue(idParent, out var list))
                {
                    list = new List<uint>();
                    ret[idParent] = list;
                }
                list.Add(process.ProcessID);
            }

            return ret;
        }

        public static IEnumerable<uint> GetChildren(this IDictionary<uint, List<uint>> processesDictionary, uint idParent)
        {
            if (processesDictionary.TryGetValue(idParent, out var children))
                foreach (var idChild in children)
                    yield return idChild;
        }

        public static IEnumerable<uint> GetDeepChildren(this IDictionary<uint, List<uint>> processesDictionary, uint idParent)
        {
            return GetDeepChildren(processesDictionary, idParent, false);
        }

        public static IEnumerable<uint> GetDeepChildren(this IDictionary<uint, List<uint>> processesDictionary, uint idParent, bool includeArgument)
        {
            List<uint> ret = new List<uint>();
            if (includeArgument) ret.Add(idParent);
            EnumSubTree(ret, processesDictionary, idParent);
            return ret;
        }

        private static void EnumSubTree(List<uint> deepChildren, IDictionary<uint, List<uint>> processesDictionary, uint idParent)
        {
            if (processesDictionary.TryGetValue(idParent, out var children))
            {
                foreach (var idChild in children)
                {
                    deepChildren.Add(idChild);
                    EnumSubTree(deepChildren, processesDictionary, idChild);
                }
            }
        }
    }
}