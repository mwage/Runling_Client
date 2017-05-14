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
            GameControl.CameraRange = 15;
            GameControl.GameActive = true;
            GameControl.TotalScore = 0;
            if (GameControl.SetGameMode == GameControl.Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            InitializeGameSLA.InitializeGame();
        }


        private void Update()
        {
            if (GameControl.IsDead && !StopUpdate)
            {
                DeathSla.Death();

                //in case of highscore, save and 
                if (GameControl.SetGameMode != GameControl.Gamemode.Practice)
                {
                    ScoreSla.SetHighScore();
                }
                
                //change level
                LevelManager.EndLevel(0.3f);

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
                if (InitializeGameSLA.Player != null)
                {
                    InitializeGameSLA.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Press 2 to turn off Godmode
            if (InputManager.Instance.GetButtonDown(HotkeyAction.DeactiveGodmode) && GameControl.GodModeActive)
            {
                GameControl.GodModeActive = false;
                if (InitializeGameSLA.Player != null)
                {
                    InitializeGameSLA.Player.transform.Find("GodMode").gameObject.SetActive(false);
                }
            }
        }
    }
}

