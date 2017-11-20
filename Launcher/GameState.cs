using Characters.Repositories;
using System.Collections.Generic;
using UnityEngine;

namespace Launcher
{
    public class GameState
    {
        public bool Solo { get; set; } = false;

        // Level/Game management (remove when refactoring RLR)
        public int CurrentLevel { get; set; } = 1;
        public bool FinishedLevel { get; set; } = false;

        // MapState (remove when refactoring RLR)
        public List<GameObject> SafeZones { get; set; }
        public GameObject DronesParent { get; set; }

        // Voting selection (maybe substitute via inscene vote later on)
        public Difficulty SetDifficulty { get; set; } = Difficulty.Hard;
        public GameMode SetGameMode { get; set; } = GameMode.Practice;
        public CharacterDto CharacterDto { get; set; }
    }

    public enum Difficulty : byte
    {
        Normal,
        Hard
    }

    public enum GameType : byte
    {
        Arena,
        RunlingRun
    }

    public enum GameMode : byte
    {
        Classic,
        TimeMode,
        Practice,
        Team
    }

    public enum PlayerColor : byte
    {
        Green,
        Red,
        Blue
    }
}
