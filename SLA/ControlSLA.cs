using Launcher;
using Network.Synchronization.Data;
using Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public GameObject PracticeMode;

        public Dictionary<uint, PlayerManager> PlayerManagers { get; } = new Dictionary<uint, PlayerManager>();
        public int CurrentLevel { get; set; } = 1;
        public Coroutine PrepareLevelCoroutine { get; private set; }

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

            var playerManager = _initializeGame.InitializePlayer(new Player(0, PlayerPrefs.GetString("username"), PlayerColor.Green));
            _initializeGame.InitializeControls(playerManager);
            playerManager.PlayerMovement = playerManager.gameObject.AddComponent<PlayerMovement>();
            PlayerManagers[0] = playerManager;
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
            _initializeGame.SpawnPlayer(PlayerManagers[0], Vector3.zero);
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

            _initializeGame.StartLevel(PlayerManagers[0]);
        }
    }
}