using System;
using System.Collections;
using System.Collections.Generic;
using Launcher;
using MP.TSGame.Drones;
using MP.TSGame.SLA.Levels;
using TrueSync;
using UI.SLA_Menus;
using UnityEngine;

namespace MP.TSGame.SLA
{
    public class LevelManagerSLA : TrueSyncBehaviour
    {
        public InGameMenuManagerSLA InGameMenuManagerSLA;
        public ScoreSLA Score;
        public GameObject Win;
        public DroneFactory DroneFactory;

        private InitializeGameSLA _initializeGameSLA;
        public static int NumLevels = 13;             //currently last level available in SLA
        private List<ILevelSLA> _levels;
    

        public void Awake()
        {
            _initializeGameSLA = GetComponent<InitializeGameSLA>();
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            _levels = new List<ILevelSLA>
            {
                new Level1SLA(this),
                new Level2SLA(this),
                new Level3SLA(this),
                new Level4SLA(this),
                new Level5SLA(this),
                new Level6SLA(this),
                new Level7SLA(this),
                new Level8SLA(this),
                new Level9SLA(this),
                new Level10SLA(this),
                new Level11SLA(this),
                new Level12SLA(this),
                new Level13SLA(this)
            };
        }

        //Spawn Drones according to what level is active
        public void LoadDrones(int level)
        {
            try
            {
                _levels[level - 1].CreateDrones();
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load level " + level + ": " + e.Message + " - " + e.StackTrace);
                PhotonNetwork.LeaveRoom();
            }
        }

        public FP GetMovementSpeed(int level)
        {
            return _levels[level - 1].GetMovementSpeed();
        }
    
        //Load next level or end game
        public void EndLevel(FP delay)
        {
            TrueSyncManager.SyncedStartCoroutine((GameControl.GameState.CurrentLevel == _levels.Count && GameControl.GameState.SetGameMode != GameMode.Practice) ? EndGameSLA(delay) : NextLevel(delay));
        }

        //load in all but the last level
        private IEnumerator NextLevel(FP delay)
        {
            yield return delay;

            Score.NewHighScore.transform.parent.gameObject.SetActive(false);

            foreach (Transform child in DroneFactory.transform)
            {
                TrueSyncManager.SyncedDestroy(child.gameObject);
            }

            yield return delay;

            foreach (var text in Score.CurrentScoreText)
            {
                text.text = "0";
            }
            if (GameControl.GameState.SetGameMode != GameMode.Practice)
            {
                GameControl.GameState.CurrentLevel++;
            }

            _initializeGameSLA.InitializeGame();
        }

        //load win screen after the last level
        private IEnumerator EndGameSLA(FP delay)
        {                
            yield return delay;
            Score.NewHighScore.transform.parent.gameObject.SetActive(false);
            InGameMenuManagerSLA.CloseMenus();
            Win.gameObject.SetActive(true);
        }
    }
}
