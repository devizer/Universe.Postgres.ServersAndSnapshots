namespace Universe.PrototypingSources
{
    using System;
    using System.Collections.Generic;

    public class WorldRegion
    {
        public string Id { get; set; }
        public string IdParent { get; set; }

        public WorldRegion Parent { get; set; }

        public string Name { get; set; }

        public List<CountryWithFlag> CountriesWithFlag { get; set; }

        public bool IsRoot
        {
            get { return IdParent == null; }
        }

        public override string ToString()
        {
            return string.Format(@"Id: {0}, IdParent: {1}, Name: {2}", Id, IdParent, Name);
        }

        // For Internal use Only
        public List<string> AllCountryNames { get; set; }

        // For Internal use Only
        public List<string> NotFoundCountryNames { get; set; }

        protected bool Equals(WorldRegion other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorldRegion) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}