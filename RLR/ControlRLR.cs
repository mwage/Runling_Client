using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class ControlRLR : MonoBehaviour
    {
        //attach scripts
        public LevelManagerRLR LevelManager;
        public ScoreRLR ScoreSla;
        public InitializeGameRLR InitializeGameSla;
        public DeathRLR DeathSla;

        public bool StopUpdate;
        
        void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
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
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (!GameControl.AutoClickerActive)
                    GameControl.AutoClickerActive = true;
            }

            // Press Alt to stop autoclicking
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (GameControl.AutoClickerActive)
                    GameControl.AutoClickerActive = false;
            }

            // Press 1 to be invulnerable
            if (Input.GetKeyDown(KeyCode.Alpha1) && !GameControl.GodModeActive)
            {
                GameControl.GodModeActive = true;
            }

            // Press 2 to be vulnerable
            if (Input.GetKeyDown(KeyCode.Alpha2) && GameControl.GodModeActive)
            {
                GameControl.GodModeActive = false;
            }
        }
    }
}

