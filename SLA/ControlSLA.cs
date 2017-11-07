using Launcher;
using Players;
using UnityEngine;

namespace SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public GameObject PracticeMode;

        private InitializeGameSLA _initializeGame;
        public PlayerManager PlayerManager;

        private void Awake()
        {
            _initializeGame = GetComponent<InitializeGameSLA>();
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
    }
}