﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Universe.Postgres.ServersAndSnapshots.Benchmark
{
    internal class TestUtils
    {
        public static int Port = 5432;

        public static string RootWorkFolder
        {
            get
            {

                var ret = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)

                    ? Path.Combine(
                        Path.GetPathRoot(Environment.SystemDirectory),
                        "Temp\\Postgres-Work-Folder")
                    : "/tmp/Postgres-Work-Folder";

                TryAndForget.Execute(() => Directory.CreateDirectory(ret));
                return ret;
            }
        }
    }
}