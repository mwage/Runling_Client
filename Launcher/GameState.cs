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
        Practice
    }

    public class GameState
    {
        // Level/Game management
        public bool GameActive = false;
        public int CurrentLevel = 1;
        public int TotalScore = 0;
        public bool FinishedLevel = false;
        public Difficulty SetDifficulty = Difficulty.Hard;
        public Gamemode SetGameMode = Gamemode.Practice;

        // Toggles
        public bool AutoClickerActive = false;
        public bool GodModeActive = false;

        // Player
        public GameObject Player;
        public float MoveSpeed = 0;
        public bool IsDead = true;
        public bool IsInvulnerable = false;
        public bool IsSafe = false;
        public bool IsImmobile = false;
        public int Lives = 0;
    }
}
