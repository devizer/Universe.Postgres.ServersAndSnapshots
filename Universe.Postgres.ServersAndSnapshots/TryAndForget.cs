using System;

namespace Universe.Postgres.ServersAndSnapshots
{
    public static class TryAndForget
    {
        public static T Evaluate<T>(Func<T> factory)
        {
            try
            {
                return factory();
            }
            catch
            {
                return default(T);
            }
        }

        public static bool Execute(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}