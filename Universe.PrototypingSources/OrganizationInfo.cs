namespace Universe.PrototypingSources
{
    public class OrganizationInfo
    {
        public string Id { get; set; }
        public string IdParent { get; set; }

        public string Name { get; set; }

        public bool IsLeaf { get; set; }
        public bool IsGroup { get; set; }

        public override string ToString()
        {
            string kind;
            if (IsLeaf) kind = "Org";
            else kind = IdParent == null ? "Root Group" : "Group";
            return
                kind + " #" + Id + ": '" + Name + "'" + (IdParent != null ? (" (parent is " + IdParent + ")") : "");
        }

        protected bool Equals(OrganizationInfo other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrganizationInfo) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}
