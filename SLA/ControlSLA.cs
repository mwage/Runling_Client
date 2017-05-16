using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public LevelManagerSLA LevelManager;
        public ScoreSLA ScoreSla;
        public InitializeGameSLA InitializeGameSLA;
        public DeathSLA DeathSla;
        public GameObject PracticeMode;

        public bool StopUpdate;

        void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.Instance.Settings.CameraRange = 15;
            GameControl.Instance.State.GameActive = true;
            GameControl.Instance.State.TotalScore = 0;
            if (GameControl.Instance.State.SetGameMode == Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            InitializeGameSLA.InitializeGame();
        }


        private void Update()
        {
            if (GameControl.Instance.State.IsDead && !StopUpdate)
            {
                DeathSla.Death();

                //in case of highscore, save and 
                if (GameControl.Instance.State.SetGameMode != Gamemode.Practice)
                {
                    ScoreSla.SetHighScore();
                }
                
                //change level
                LevelManager.EndLevel(0.3f);

                //dont repeat above once player dead
                StopUpdate = true;
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

            // Press 1 to turn on Godmode
            if (GameControl.Instance.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.Instance.State.GodModeActive)
            {
                GameControl.Instance.State.GodModeActive = true;
                if (GameControl.Instance.State.Player != null)
                {
                    GameControl.Instance.State.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Press 2 to turn off Godmode
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

