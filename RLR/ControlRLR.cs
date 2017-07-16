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
            GameControl.State.GameActive = true;
            GameControl.State.MoveSpeed = 13;
            GameControl.State.TotalScore = 0;
            if (GameControl.State.SetGameMode == Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            if (GameControl.State.SetGameMode == Gamemode.TimeMode)
            {
                GameControl.State.Lives = 3;
                TimeModeUI.SetActive(true);
                CountDownText.GetComponent<TextMeshProUGUI>().text = "Countdown: " + (int)((285 + GameControl.State.CurrentLevel*15) / 60) + ":" + ((285 + GameControl.State.CurrentLevel*15) % 60).ToString("00.00");
                HighScoreText.GetComponent<TextMeshProUGUI>().text = GameControl.State.SetDifficulty == Difficulty.Normal ? "Highscore: " + GameControl.HighScores.HighScoreRLRNormal[0].ToString("f0") : "Highscore: " + GameControl.HighScores.HighScoreRLRHard[0].ToString("f0");
                LevelManager.LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + GameControl.State.Lives;
            }

            InitializeGameRLR.InitializeGame();
        }

        //update when dead
        private void Update()
        {
            if (GameControl.State.IsDead && !StopUpdate)
            {
                StopUpdate = true;
                DeathRLR.Death(LevelManager, InitializeGameRLR, this);
            }

            if (GameControl.State.FinishedLevel && !StopUpdate)
            {
                StopUpdate = true;
                LevelManager.EndLevel(0);
            }

            // Start autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                if (!GameControl.State.AutoClickerActive)
                    GameControl.State.AutoClickerActive = true;
            }

            // Stop autoclicking
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                if (GameControl.State.AutoClickerActive)
                    GameControl.State.AutoClickerActive = false;
            }
            /*
            // Become invulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.State.GodModeActive)
            {
                GameControl.State.GodModeActive = true;
                if (GameControl.PlayerState.Player != null)
                {
                    GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Become vulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.State.GodModeActive)
            {
                GameControl.State.GodModeActive = false;
                if (GameControl.PlayerState.Player != null)
                {
                    GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(false);
                }
            }
            */
        }
    }
}

