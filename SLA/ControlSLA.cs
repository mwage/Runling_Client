using Launcher;
using Players;
using System.Collections;
using System.Collections.Generic;
using Network.Synchronization.Data;
using UnityEngine;

namespace SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public GameObject PracticeMode;


        public PlayerManager PlayerManager { get; private set; }
        public Dictionary<uint, PlayerManager> PlayerManagers { get; } = new Dictionary<uint, PlayerManager>(); // for Multiplayer
        public int CurrentLevel { get; set; } = 1;

        private InitializeGameSLA _initializeGame;
        private LevelManagerSLA _levelManager;

        private void Awake()
        {
            _initializeGame = GetComponent<InitializeGameSLA>();
            _levelManager = GetComponent<LevelManagerSLA>();
        }

        private void Start()
        {
            GameControl.Settings.CameraRange = 15;

            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }

            // Only initialize on it's own when playing solo
            if (!GameControl.GameState.Solo)
                return;

            PlayerManager = _initializeGame.InitializePlayer(new Player(0, PlayerPrefs.GetString("username"), PlayerColor.Green));
            InitializeLevel();
        }

        public void InitializeLevel()
        {
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // Show Highscore and current level
            _initializeGame.PrepareLevel();
            yield return new WaitForSeconds(3);
            _initializeGame.HidePanels();
            yield return new WaitForSeconds(1);

            // Spawn Players and Drones
            _initializeGame.SpawnPlayer(PlayerManager, Vector3.zero);
            _levelManager.LoadDrones(CurrentLevel);

            yield return new WaitForSeconds(1);

            // Countdown
            for (var i = 3; i >= 0; i--)
            {
                _initializeGame.Countdown(i);

                if (i != 0)
                {
                    yield return new WaitForSeconds(1);
                }
            }

            _initializeGame.StartLevel(PlayerManager);
        }
    }
}