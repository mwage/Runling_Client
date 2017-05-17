using System.Collections.Generic;
using Launcher;
using RLR.Levels.Hard;
using RLR.Levels.Normal;

namespace RLR.Levels
{
    public class InitializeLevelsRLR
    {
        public List<ILevelRLR> SetDifficulty(LevelManagerRLR manager)
        {
            return GameControl.State.SetDifficulty == Difficulty.Hard ? SetHard(manager) : SetNormal(manager);
        }

        private static List<ILevelRLR> SetNormal(LevelManagerRLR manager)
        {
            var levels = new List<ILevelRLR>
            {
                new Level1RLR(manager),
                new Level2RLR(manager),
                new Level3RLR(manager),
                new Level4RLR(manager),
                new Level5RLR(manager),
                new Level6RLR(manager),
                new Level7RLR(manager),
                new Level8RLR(manager),
                new Level9RLR(manager)
            };
            return levels;
        }

        private static List<ILevelRLR> SetHard(LevelManagerRLR manager)
        {
            var levels = new List<ILevelRLR>
            {
                new Level1HardRLR(manager),
                new Level2HardRLR(manager),
                new Level3HardRLR(manager),
                new Level4HardRLR(manager),
                new Level5HardRLR(manager),
                new Level6HardRLR(manager),
                new Level7HardRLR(manager),
                new Level8HardRLR(manager),
                new Level9HardRLR(manager)
            };
            return levels;
        }
    }
}
