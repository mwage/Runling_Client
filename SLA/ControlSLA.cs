using Launcher;
using Players;
using UnityEngine;

namespace SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public GameObject PracticeMode;

        private LevelManagerSLA _levelManager;
        private InitializeGameSLA _initializeGame;
        private DeathSLA _death;

        public PlayerManager PlayerManager;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerSLA>();
            _initializeGame = GetComponent<InitializeGameSLA>();
            _death = GetComponent<DeathSLA>();
        }

        private void Start()
        {
            GameControl.Settings.CameraRange = 15;
            GameControl.GameState.GameActive = true;

            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }

            _initializeGame.InitializePlayer();
            _initializeGame.InitializeGame();
        }

       private void Update()
        {
            if (PlayerManager != null)
                CheckIfDead();
        }

        private void CheckIfDead()
        {
            if (PlayerManager.CheckIfDead && PlayerManager.IsDead)
            {
                _death.Death(PlayerManager);
                _levelManager.EndLevel(1);
            }
        }
    }
}