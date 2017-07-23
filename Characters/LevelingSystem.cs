using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Launcher;

namespace Characters
{
    public static class LevelingSystem
    {
        public static readonly int MaxCharactersAmount = 8;
        public static readonly int CharactersAmount = 8;
        public static readonly int MaxLevel = 50;
        // LevelExperienceCurve[1] tells about exp you must exceed to be promoted at 1 level (to gain 2nd Level)
        public static readonly List<int> LevelExperienceCurve = new List<int>{ 0, 1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 167, 217, 271, 329, 391, 457, 527, 601, 679, 761, 2234, 3843, 5594, 7493, 9546, 11759, 14138, 16689, 19418, 22331, 25434, 28733, 32234, 35943, 39866, 44009, 48378, 52979, 57818, 62901, 68234, 73823, 79674, 85793, 92186, 98859, 105818, 113069, 120618 };
        public static readonly int NewPointsPerLevel = 5;
        public static readonly int MaximumPoints = 80;
        public static readonly int AbilityMaxLevel = 3;
        public static readonly int[] PropertyCostModifierLevels = {15, 25};


        public static readonly int AbilityPointsCostPerLevel = 5;

        public static int CalculateExp(int platformIndex, int level, Difficulty difficulty, GameMode mode)
        {
            if (platformIndex == 0) return 0;

            int difficlutyMultiplier = difficulty == Difficulty.Hard ? 2 : 1;
            if (platformIndex == GameControl.MapState.SafeZones.Count - 1) difficlutyMultiplier *= 10;

            if (mode == GameMode.Practice)
            {
                return level * difficlutyMultiplier; // set to 0 later
            }
            
            return level * difficlutyMultiplier;

            
        }
    }
}
