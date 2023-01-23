namespace Universe.PrototypingSources
{
    public class HumanName
    {
        public string Name { get; set; }
        public int Popularity { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }

        public HumanName()
        {
        }

        public HumanName(string name, int popularity, Gender gender, string description)
        {
            Name = name;
            Popularity = popularity;
            Gender = gender;
            Description = description;
        }

        public bool IsPetForm
        {
            get { return Description.StartsWith("Pet"); }
        }

        protected bool Equals(HumanName other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HumanName) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }

    public enum Gender
    {
        Male,
        Female,
    }

}