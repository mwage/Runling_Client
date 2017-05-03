using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class ControlRLR : MonoBehaviour
    {
        public LevelManagerRLR LevelManager;
        public InitializeGameRLR InitializeGameRLR;
        public DeathRLR DeathRLR;

        public bool StopUpdate;

        void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.GameActive = true;
            GameControl.CurrentLevel = 1;
            GameControl.MoveSpeed = 13;
            InitializeGameRLR.InitializeGame();
        }

        //update when dead
        private void Update()
        {
            if (GameControl.Dead && !StopUpdate)
            {
                DeathRLR.Death();

                //change level
                LevelManager.EndGame(3f);

                //dont repeat above once player dead
                StopUpdate = true;
            }

            if (GameControl.FinishedLevel && !StopUpdate)
            {
                LevelManager.EndLevel(0f);
            }


            // Press Ctrl to start autoclicking
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ActivateClicker))
            {
                if (!GameControl.AutoClickerActive)
                    GameControl.AutoClickerActive = true;
            }

            // Press Alt to stop autoclicking
            if (InputManager.Instance.GetButtonDown(HotkeyAction.DeactivateClicker))
            {
                if (GameControl.AutoClickerActive)
                    GameControl.AutoClickerActive = false;
            }

            // Press 1 to be invulnerable
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.GodModeActive)
            {
                GameControl.GodModeActive = true;
            }

            // Press 2 to be vulnerable
            if (InputManager.Instance.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.GodModeActive)
            {
                GameControl.GodModeActive = false;
            }
        }
    }
}

