using Launcher;
using SLA.Levels;
using UnityEngine;

namespace SLA
{
    public class ControlSLA : MonoBehaviour
    {
        public LevelManagerSLA LevelManager;
        public ScoreSLA ScoreSla;
        public InitializeGameSLA InitializeGameSLA;
        public DeathSLA DeathSla;
        public GameObject PracticeMode;

        public bool StopUpdate;

        private void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            GameControl.Settings.CameraRange = 15;
            GameControl.State.GameActive = true;
            GameControl.State.TotalScore = 0;
            if (GameControl.State.SetGameMode == Gamemode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            InitializeGameSLA.InitializeGame();
        }

        private void Update()
        {
            if (GameControl.PlayerState.IsDead && !StopUpdate)
            {
                DeathSla.Death();

                //in case of highscore, save and 
                if (GameControl.State.SetGameMode != Gamemode.Practice)
                {
                    ScoreSla.SetHighScore();
                }
                
                //change level
                LevelManager.EndLevel(2f);

                //dont repeat above once player dead
                StopUpdate = true;
            }

            //// Press Ctrl to start autoclicking // TODO: delete if works in InputServer
            //if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateClicker))
            //{
            //    if (!GameControl.State.AutoClickerActive)
            //        GameControl.State.AutoClickerActive = true;
            //}

            //// Press Alt to stop autoclicking
            //if (GameControl.InputManager.GetButtonDown(HotkeyAction.DeactivateClicker))
            //{
            //    if (GameControl.State.AutoClickerActive)
            //        GameControl.State.AutoClickerActive = false;
            //} ------------------------------------------------------------------------------------------------
            
            /*
            // Press 1 to turn on Godmode
            if (GameControl.InputManager.GetButtonDown(HotkeyAction.ActivateGodmode) && !GameControl.State.GodModeActive)
            {
                GameControl.State.GodModeActive = true;
                if (GameControl.PlayerState.Player != null)
                {
                    GameControl.PlayerState.Player.transform.Find("GodMode").gameObject.SetActive(true);
                }
            }

            // Press 2 to turn off Godmode
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

