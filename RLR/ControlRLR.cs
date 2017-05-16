using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class ControlRLR : MonoBehaviour
    {
        public LevelManagerRLR LevelManager;
        public InitializeGameRLR InitializeGameRLR;
        public DeathRLR DeathRLR;
        public GameObject PracticeMode;

        public bool StopUpdate;

        void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.Instance.State.GameActive = true;
            GameControl.Instance.State.MoveSpeed = 13;
            if (GameControl.Instance.State.SetGameMode == Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
            }

            InitializeGameRLR.InitializeGame();
        }

        //update when dead
        private void Update()
        {
            if (GameControl.Instance.State.IsDead && !StopUpdate)
            {
                StopUpdate = true;
                DeathRLR.Death(LevelManager, InitializeGameRLR, this);
            }

            if (GameControl.Instance.State.FinishedLevel && !StopUpdate)
            {
                LevelManager.EndLevel(0f);
            }

            // Press Ctrl to start autoclicking
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                if (!GameControl.Instance.State.AutoClickerActive)
                    GameControl.Instance.State.AutoClickerActive = true;
            }

            // Press Alt to stop autoclicking
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                if (GameControl.Instance.State.AutoClickerActive)
                    GameControl.Instance.State.AutoClickerActive = false;
            }

            // Press 1 to be invulnerable
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.Instance.State.GodModeActive)
            {
                GameControl.Instance.State.GodModeActive = true;
                if (GameControl.Instance.State.Player != null)
                {
                    GameControl.Instance.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Press 2 to be vulnerable
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.Instance.State.GodModeActive)
            {
                GameControl.Instance.State.GodModeActive = false;
                if (GameControl.Instance.State.Player != null)
                {
                    GameControl.Instance.State.Player.transform.Find("GodMode").gameObject.SetActive(false);
                }
            }
        }
    }
}

