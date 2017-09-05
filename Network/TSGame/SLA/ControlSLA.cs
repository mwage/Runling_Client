using System.Linq;
using Launcher;
using MP.TSGame.Players;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.SLA
{
    public class ControlSLA : TrueSyncBehaviour
    {
        public ScoreSLA Score;
        public GameObject PracticeMode;

        private LevelManagerSLA _levelManager;
        private InitializeGameSLA _initializeGame;
        private DeathSLA _death;
        public bool LoadingNextLevel;
        public bool CheckDeaths;
        public PlayerManager[] PlayerManager;
        public bool AllDead;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerSLA>();
            _initializeGame = GetComponent<InitializeGameSLA>();
            _death = GetComponent<DeathSLA>();
            PlayerManager = new PlayerManager[PhotonNetwork.room.PlayerCount];

//             TODO: Replace by randomly rolled seed
            GameControl.GameState.Random = TSRandom.New(0);
        }

        public override void OnSyncedStart()
        {
            GameControl.Settings.CameraRange = 15;
            GameControl.GameState.GameActive = true;

            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            _initializeGame.InitializeGame();
        }

        public override void OnSyncedUpdate()
        {
            if (CheckDeaths)
                CheckIfDead();
        }

        private void CheckIfDead()
        {
            foreach (var playerManager in PlayerManager)
            {
                if (playerManager.CheckIfDead && playerManager.IsDead)
                {
                    _death.Death(playerManager, Score);
                }
            }
            if (PlayerManager.Where(manager => manager != null).Any(manager => !manager.IsDead))
                return;

            LevelOver();
        }

        private void LevelOver()
        {
            CheckDeaths = false;
            AllDead = true;

            if (LoadingNextLevel)
                return;

            LoadingNextLevel = true;
            _levelManager.DroneFactory.LevelCounter++;
            _levelManager.EndLevel(1);
        }
    }
}

