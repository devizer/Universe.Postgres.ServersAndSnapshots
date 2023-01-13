using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class PgServerTestCase
    {
        public ServerBinaries ServerBinaries { get; set; }
        public string Locale { get; set; }


        public static PgServerTestCase[] GetServers()
        {
            List<PgServerTestCase> ret = new();
            ServerBinaries[] servers = PostgresServerDiscovery.GetServers();
            // empty locale means no parameter
            var locales = GetEnvLocales() ?? new[] {""};
            foreach (var locale in locales)
                foreach (var server in servers)
                    ret.Add(new PgServerTestCase() { Locale = locale, ServerBinaries = server});

            return ret.ToArray();
        }

        static string[] GetEnvLocales()
        {
            // dash (-) locale means no parameter
            var raw = Environment.GetEnvironmentVariable("PG_SERVER_LOCALES");
            if (string.IsNullOrEmpty(raw)) return null;
            return
                raw
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x == "-" ? "" : x)
                    .ToArray();
        }

        public override string ToString()
        {
            return 
                (string.IsNullOrEmpty(Locale) ? "[Default Locale] " : $"[{Locale}] ")
                + ServerBinaries.ToString();
        }
    }
}