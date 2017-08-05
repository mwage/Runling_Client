using System;
using System.Collections;
using System.Collections.Generic;
using Launcher;
using Drones;
using SLA.Levels;
using UI.SLA_Menus;
using UnityEngine;

namespace SLA
{
    public class LevelManagerSLA : MonoBehaviour
    {
        public InGameMenuManagerSLA InGameMenuManagerSLA;
        private ScoreSLA _score;
        public GameObject Win;
        public DroneFactory DroneFactory;

        private InitializeGameSLA _initializeGameSLA;
        public static int NumLevels = 13;             //currently last level available in SLA
        private List<ILevelSLA> _levels;


        public void Awake()
        {
            _score = GetComponent<ScoreSLA>();
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

        public float GetMovementSpeed(int level)
        {
            return _levels[level - 1].GetMovementSpeed();
        }

        //Load next level or end game
        public void EndLevel(float delay)
        {
            StartCoroutine((GameControl.GameState.CurrentLevel == _levels.Count && GameControl.GameState.SetGameMode != GameMode.Practice) ? EndGameSLA(delay) : NextLevel(delay));
        }

        //load in all but the last level
        private IEnumerator NextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);

            _score.NewHighScore.transform.parent.gameObject.SetActive(false);

            foreach (Transform child in DroneFactory.transform)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitForSeconds(delay);

            _score.CurrentScoreText.text = "0";
            if (GameControl.GameState.SetGameMode != GameMode.Practice)
            {
                GameControl.GameState.CurrentLevel++;
            }

            _initializeGameSLA.InitializeGame();
        }

        //load win screen after the last level
        private IEnumerator EndGameSLA(float delay)
        {
            yield return new WaitForSeconds (delay);
            _score.NewHighScore.transform.parent.gameObject.SetActive(false);
            InGameMenuManagerSLA.CloseMenus();
            Win.gameObject.SetActive(true);
        }
    }
}
