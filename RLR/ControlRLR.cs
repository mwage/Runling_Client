using Launcher;
using RLR.Levels;
using TMPro;
using UnityEngine;

namespace RLR
{
    public class ControlRLR : MonoBehaviour
    {
        public LevelManagerRLR LevelManager;
        public InitializeGameRLR InitializeGameRLR;
        public DeathRLR DeathRLR;
        public GameObject PracticeMode;
        public GameObject TimeModeUI;
        public GameObject CountDownText;
        public GameObject HighScoreText;

        public bool StopUpdate;

        private void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.GameState.GameActive = true;
            GameControl.PlayerState.TotalScore = 0;

            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
                GameControl.PlayerState.Lives = 3;
                TimeModeUI.SetActive(true);
                CountDownText.GetComponent<TextMeshProUGUI>().text = "Countdown: " + (int)((285 + GameControl.GameState.CurrentLevel*15) / 60) + ":" + ((285 + GameControl.GameState.CurrentLevel*15) % 60).ToString("00.00");
                HighScoreText.GetComponent<TextMeshProUGUI>().text = GameControl.GameState.SetDifficulty == Difficulty.Normal ? "Highscore: " + GameControl.HighScores.HighScoreRLRNormal[0].ToString("f0") : "Highscore: " + GameControl.HighScores.HighScoreRLRHard[0].ToString("f0");
                LevelManager.LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + GameControl.PlayerState.Lives;
            }

            InitializeGameRLR.InitializeGame();
        }

        //update when dead
        private void Update()
        {
            if (GameControl.PlayerState.IsDead && !StopUpdate)
            {
                StopUpdate = true;
                DeathRLR.Death(LevelManager, InitializeGameRLR, this);
            }

            if (GameControl.GameState.FinishedLevel && !StopUpdate)
            {
                StopUpdate = true;
                LevelManager.EndLevel(0);
            }
        }
    }
}

