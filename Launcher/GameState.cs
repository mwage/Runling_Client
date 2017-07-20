using Characters.Types;
using UnityEngine;

namespace Launcher
{
    public enum Difficulty
    {
        Normal,
        Hard
    }

    public enum Gamemode
    {
        Classic,
        TimeMode,
        Practice,
        Team
    }

    public class GameState
    {
        // Level/Game management
        public bool GameActive = false;
        public int CurrentLevel = 1;
        public bool FinishedLevel = false;
        public Difficulty SetDifficulty = Difficulty.Hard;
        public Gamemode SetGameMode = Gamemode.Practice;
        public bool Solo;
    }
}
