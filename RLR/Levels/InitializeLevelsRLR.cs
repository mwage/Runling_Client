using System.Collections.Generic;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class InitializeLevelsRLR
    {
        public List<ILevelRLR> SetDifficulty(LevelManagerRLR manager)
        {
            List<ILevelRLR> levels;

            if (GameControl.Instance.State.SetDifficulty == Difficulty.Hard)
            {
                levels = SetHard(manager);
            }
            else
            {
                levels = SetNormal(manager);
            }

            return levels;
        }


        List<ILevelRLR> SetNormal(LevelManagerRLR manager)
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

        List<ILevelRLR> SetHard(LevelManagerRLR manager)
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
