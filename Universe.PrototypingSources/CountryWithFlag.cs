namespace Universe.PrototypingSources
{
    using System;
    using System.Reflection;
    using System.Resources;

    public class CountryWithFlag
    {
        public string Name { get; set; }
        public string WikiUrl { get; set; }
        public decimal GDP { get; set; }

        public string FlagKey { get; set; }

        public string SvgFlagUrl { get; set; }
        public int FlagWidth { get; set; }
        public int FlagHeight { get; set; }


        public string FlagAsSvg
        {
            get
            {
                string ret = GetResMan().GetString(this.FlagKey);
                if (ret == null)
                    throw new InvalidOperationException(string.Format(
                        "It seems generated data is corrupter. Flag of {0} is not found in {1} by '{2}' key.",
                        Name, ResourceKey, FlagKey));

                return ret;
            }
        }

        protected bool Equals(CountryWithFlag other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CountryWithFlag) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        private const string ResourceKey = "Universe.PrototypeSources.Wiki_Flags_Of_Countries";
        private static ResourceManager _ResMan;
        static readonly object _Sync = new object();

        static ResourceManager GetResMan()
        {
            lock (_Sync)
            {
                if (_ResMan != null) return _ResMan;
                var assembly = GetAssemblyByType(typeof(CountryWithFlag));
                foreach (string name in assembly.GetManifestResourceNames())
                {
                    if (name.EndsWith(ResourceKey + ".resources"))
                    {
                        string baseName = name.Substring(0, name.Length - ".resources".Length);
                        ResourceManager m = new ResourceManager(baseName, assembly);
                        _ResMan = m;
                        return _ResMan;
                    }
                }

                throw new InvalidOperationException(string.Format(
                    "Embedded resource {0} was not found in any folder of Assembly {1}",
                    ResourceKey, assembly.FullName));
            }

        }

        static Assembly GetAssemblyByType(Type type)
        {
#if NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0
            Assembly assembly = type.GetTypeInfo().Assembly;
#elif NET20 || NET35 || NET40 
            Assembly assembly = type.Assembly;
#else
            Assembly assembly = type.GetTypeInfo().Assembly;
#endif
            return assembly;
        }
    }
}

