using Characters.Types;
using TrueSync;
using UnityEngine;

namespace Launcher
{
    public enum Difficulty
    {
        Normal,
        Hard
    }

    public enum GameMode
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
        public bool AllDead = false;
        public Difficulty SetDifficulty = Difficulty.Hard;
        public GameMode SetGameMode = GameMode.Practice;
        public bool Solo;
        public TSRandom Random = TSRandom.New(0);
    }
}
