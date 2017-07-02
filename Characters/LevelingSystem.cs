using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Characters
{
    public static class LevelingSystem
    {
        public static readonly int MaxCharactersAmount = 8;
        public static readonly int CharactersAmount = 8;
        public static readonly int MaxLevel = 50;
        // LevelExperienceCurve[1] tells about exp you must exceed to be promoted at 1 level (to gain 2nd Level)
        public static readonly List<int> LevelExperienceCurve = new List<int>{ 0, 0, 1, 5, 12, 25, 48, 81, 131, 199, 292, 414, 571, 768, 1012, 1310, 1670, 2099, 2606, 3199, 3889, 4685, 5596, 6635, 7812, 9139, 10628, 12293, 14145, 16199, 18470, 20971, 23718, 26726, 30012, 33592, 37483, 41702, 46268, 51199, 56515, 62233, 68376, 74961, 82012, 89549, 97593, 106168, 115296, 124999 };
        public static readonly int NewPointsPerLevel = 5;
        public static readonly int MaximumPoints = 50;
        public static readonly int AbilityMaxLevel = 3;
        
        public static readonly int AbilityPointsCostPerLevel = 5;
    }
}
