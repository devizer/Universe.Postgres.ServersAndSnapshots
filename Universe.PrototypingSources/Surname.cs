namespace Universe.PrototypingSources
{
    public class Surname
    {
        public string FamilyName { get; set; }
        public HumanRace Race { get; set; }
        public int RankInRace { get; set; }
        public int RankInTotal { get; set; }
        public int SelfIdentifyingTotal { get; set; }
        public decimal SelfIdentifyingPerCent { get; set; }
        public int TotalOccurence { get; set; }

        public Surname()
        {
        }

        public Surname(string familyName, HumanRace race, int rankInRace, int rankInTotal, int selfIdentifyingTotal, decimal selfIdentifyingPerCent, int totalOccurence)
        {
            FamilyName = familyName;
            Race = race;
            RankInRace = rankInRace;
            RankInTotal = rankInTotal;
            SelfIdentifyingTotal = selfIdentifyingTotal;
            SelfIdentifyingPerCent = selfIdentifyingPerCent;
            TotalOccurence = totalOccurence;
        }

        protected bool Equals(Surname other)
        {
            return string.Equals(FamilyName, other.FamilyName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Surname) obj);
        }

        public override int GetHashCode()
        {
            return (FamilyName != null ? FamilyName.GetHashCode() : 0);
        }
    }

    public enum HumanRace
    {
        Asian,
        Black,
        Hispanic,
        NativeAmerican,
        TwoOrMoreRaces,
        White
    }
}