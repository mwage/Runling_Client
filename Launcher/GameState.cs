using System.Collections.Generic;
using Characters.Types;
using TrueSync;
using UnityEngine;

namespace Launcher
{
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

    public class GameState
    {
        // Level/Game management
        public bool GameActive = false;
        public int CurrentLevel = 1;
        public bool FinishedLevel = false;

        // MapState
        public List<GameObject> SafeZones;
        public GameObject DronesParent;

        // TS random placeholder
        public TSRandom Random;

        // Voting selection (mabe substitute via inscene vote later on)
        public Difficulty SetDifficulty = Difficulty.Hard;
        public GameMode SetGameMode = GameMode.Practice;
        public CharacterDto CharacterDto;
    }
}
