using Launcher;

namespace Characters
{
    public static class LevelingSystem
    {
        public const int MaxCharactersAmount = 8;
        public const int MaxLevel = 50;
        // LevelExperienceCurve[1] tells about exp you must exceed to be promoted at 1 level (to gain 2nd Level)
        public static int[] LevelExperienceCurve { get; } = { 0, 2, 6, 14, 29, 53, 87, 134, 196, 274, 371, 488, 628, 793, 984, 1204, 1455, 1738, 2056, 2411, 2804, 3238, 3714, 4235, 4803, 5419, 6086, 6806, 7580, 8411, 9300, 10250, 11263, 12340, 13484, 14697, 15980, 17336, 18767, 20274, 21860, 23526, 25275, 27109, 29029, 31038, 33138, 35330, 37617, 40000, 45039, 50281, 55730, 61390, 67265, 73358, 79674, 86217, 92991, 100000};
        public const int NewPointsPerLevel = 4;
        public const int MaximumPoints = 100;
        public const int AbilityMaxLevel = 5;
        public static int[] PropertyCostModifierLevels { get; } = {60, 80};


        public static readonly int AbilityPointsCostPerLevel = 5;

        public static int CalculateExp(int platformIndex, int level, Difficulty difficulty, GameMode mode)
        {
            if (platformIndex == 0 || mode == GameMode.Practice)
                return 0;

            var difficlutyMultiplier = difficulty == Difficulty.Hard ? 2 : 1;
         
            return level * difficlutyMultiplier;
        }
    }
}
