using System;
using System.IO;

namespace Universe.Postgres.ServersAndSnapshots
{
    internal class DisposableTempFile : IDisposable
    {
        public string FullPath { get; }

        private DisposableTempFile(string fullPath)
        {
            FullPath = fullPath;
        }

        public static IDisposable Create(string fullPath, string content)
        {
            File.WriteAllText(fullPath, content);
            return new DisposableTempFile(fullPath);
        }

        public void Dispose()
        {
            TryAndForget.Execute(() => File.Delete(FullPath));
        }
    }
}
