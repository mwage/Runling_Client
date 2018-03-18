using Game.Scripts.Characters.CharacterRepositories;
using Game.Scripts.GameSettings;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Launcher
{
    public class GameState
    {
        public bool Solo { get; set; } = false;

        // Level/Game management (remove when refactoring RLR)
        public bool FinishedLevel { get; set; } = false;

        // MapState (remove when refactoring RLR)
        public List<GameObject> SafeZones { get; set; }
        public GameObject DronesParent { get; set; }

        // Voting selection (maybe substitute via inscene vote later on)
        public Difficulty SetDifficulty { get; set; } = Difficulty.Hard;
        public GameMode SetGameMode { get; set; } = GameMode.Practice;
        public CharacterDto CharacterDto { get; set; }
    }
}
