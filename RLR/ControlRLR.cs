using Launcher;
using Players;
using RLR.Levels;
using TMPro;
using UnityEngine;

namespace RLR
{
    public class ControlRLR : MonoBehaviour
    {
        public GameObject PracticeMode;
        public GameObject TimeModeUI;
        public GameObject CountDownText;
        public GameObject HighScoreText;

        public PlayerManager PlayerManager;

        private LevelManagerRLR _levelManager;
        private InitializeGameRLR _initializeGame;
        private DeathRLR _death;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerRLR>();
            _initializeGame = GetComponent<InitializeGameRLR>();
            _death = GetComponent<DeathRLR>();

            PlayerTrigger.onFinishedLevel += FinishedLevel;
        }

        private void OnDestroy()
        {
            PlayerTrigger.onFinishedLevel -= FinishedLevel;
        }

        private void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            GameControl.GameState.GameActive = true;

            _initializeGame.InitializePlayer();

            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }

            if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
                PlayerManager.Lives = 3;
                TimeModeUI.SetActive(true);

                CountDownText.GetComponent<TextMeshProUGUI>().text = 
                    "Countdown: " + (int)((285 + GameControl.GameState.CurrentLevel*15) / 60) + ":" + 
                    ((285 + GameControl.GameState.CurrentLevel*15) % 60).ToString("00.00");

                HighScoreText.GetComponent<TextMeshProUGUI>().text = GameControl.GameState.SetDifficulty == 
                    Difficulty.Normal ? "Highscore: " + GameControl.HighScores.HighScoreRLRNormal[0].ToString("f0") : 
                    "Highscore: " + GameControl.HighScores.HighScoreRLRHard[0].ToString("f0");

                _levelManager.LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + PlayerManager.Lives;
            }

            _initializeGame.InitializeGame();
        }

        public void FinishedLevel()
        {
            _levelManager.EndLevel(0);
        }
    }
}