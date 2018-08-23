using System.Collections.Generic;

namespace SWD2Randomizer
{
    public delegate bool Access(List<string> have);
    public class Location
    {
        public enum RandomizeType
        {
            None,
            Upgrade
        }

        public RandomizeType Type { get; set; }
        public string Name { get; set; }
        public Access CanAccess { get; set; }
        public string Grant { get; set; }
        public Access CanEscape { get; set; }
        public Access CanEscapeWithoutNew { get; set; }

        public Location()
        {
            Type = RandomizeType.None;
            CanAccess = have => true;
            CanEscape = have => true;
            CanEscapeWithoutNew = have => false;
        }
    }
}
 