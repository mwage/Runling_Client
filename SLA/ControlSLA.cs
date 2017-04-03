using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.SLA
{
    public class ControlSLA : MonoBehaviour
    {
        //attach scripts
        public LevelManagerSLA LevelManager;
        public ScoreSLA ScoreSla;
        public InitializeGameSLA InitializeGameSla;
        public DeathSLA DeathSla;

        public bool StopUpdate;
        
        void Start()
        {
            // Set current Level and movespeed, load drones and spawn immunity
            StopUpdate = true;
            InitializeGameSla.InitializeGame();
            //GameControl.currentLevel = 10;
        }

        //update when dead
        private void Update()
        {
            if (GameControl.dead && !StopUpdate)
            {
                DeathSla.Death();

                //in case of highscore, save and 
                ScoreSla.SetLevelHighScore();

                //change level
                LevelManager.EndLevel(3f);

                //dont repeat above once player dead
                StopUpdate = true;
            }
        }
    }
}

