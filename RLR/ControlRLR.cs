using Launcher;
using RLR.Levels;
using UnityEngine;

namespace RLR
{
    public class ControlRLR : MonoBehaviour
    {
        public LevelManagerRLR LevelManager;
        public InitializeGameRLR InitializeGameRLR;
        public DeathRLR DeathRLR;
        public GameObject PracticeMode;

        public bool StopUpdate;

        private void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.State.GameActive = true;
            GameControl.State.MoveSpeed = 13;
            if (GameControl.State.SetGameMode == Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
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
                LevelManager.EndLevel(0f);
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

            // Become invulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.State.GodModeActive)
            {
                GameControl.State.GodModeActive = true;
                if (GameControl.State.Player != null)
                {
                    GameControl.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Become vulnerable
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.State.GodModeActive)
            {
                GameControl.State.GodModeActive = false;
                if (GameControl.State.Player != null)
                {
                    GameControl.State.Player.transform.Find("GodMode").gameObject.SetActive(false);
                }
            }
        }
    }
}

