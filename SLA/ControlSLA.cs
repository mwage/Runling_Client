using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public LevelManagerSLA LevelManager;
        public ScoreSLA ScoreSla;
        public InitializeGameSLA InitializeGameSla;
        public DeathSLA DeathSla;

        public bool StopUpdate;

        void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.CameraRange = 15;
            GameControl.GameActive = true;
            GameControl.CurrentLevel = 1;
            InitializeGameSla.InitializeGame();
        }

        //update when dead
        private void Update()
        {
            if (GameControl.Dead && !StopUpdate)
            {
                DeathSla.Death();

                //in case of highscore, save and 
                ScoreSla.SetHighScore();

                //change level
                LevelManager.EndLevel(3f);

                //dont repeat above once player dead
                StopUpdate = true;
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

            // Press 1 to turn on Godmode
            if (InputManager.Instance.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.GodModeActive)
            {
                GameControl.GodModeActive = true;
            }

            // Press 2 to turn off Godmode
            if (InputManager.Instance.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.GodModeActive)
            {
                GameControl.GodModeActive = false;
            }
        }
    }
}

