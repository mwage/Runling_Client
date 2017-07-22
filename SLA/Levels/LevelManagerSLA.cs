using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Drones;
using Launcher;
using UI.SLA_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLA.Levels
{
    public class LevelManagerSLA : MonoBehaviour {

        public InGameMenuManagerSLA InGameMenuManagerSLA;
        public ScoreSLA Score;
        public InitializeGameSLA InitializeGameSLA;
        public DroneFactory DroneFactory;

        public GameObject Win;
        public static int NumLevels = 13;             //currently last level available in SLA
        private List<ILevelSLA> _levels;

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

        public void Awake()
        {
            if (PhotonNetwork.isMasterClient)
            {
                DroneFactory = PhotonNetwork.InstantiateSceneObject(Path.Combine("Drones", "Drone Manager"), 
                    Vector3.zero, Quaternion.identity, 0, new object[0]).GetComponent<DroneFactory>();
            }
            InitializeLevels();
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
                SceneManager.LoadScene("MainMenu");
            }
        }

        public float GetMovementSpeed(int level)
        {
            return _levels[level - 1].GetMovementSpeed();
        }
    
        //Load next level or end game
        public void EndLevel(float delay)
        {
            StartCoroutine((GameControl.GameState.CurrentLevel == _levels.Count && GameControl.GameState.SetGameMode != Gamemode.Practice) ? EndGameSLA(delay) : NextLevel(delay));
        }

        //load in all but the last level
        private IEnumerator NextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            Score.HighScore.SetActive(false);
            DroneFactory.StopAllCoroutines();
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var t in enemies)
            {
                Destroy(t);
            }
            Destroy(GameControl.PlayerState.Player);
            Score.CurrentScoreText.text = "0";
            if (GameControl.GameState.SetGameMode != Gamemode.Practice)
            {
                GameControl.GameState.CurrentLevel++;
            }

            InitializeGameSLA.InitializeGame();
        }

        //load after the last level
        private IEnumerator EndGameSLA(float delay)
        {                
            //load win screen
            yield return new WaitForSeconds(delay);
            Score.HighScore.SetActive(false);
            InGameMenuManagerSLA.CloseMenus();
            Win.gameObject.SetActive(true);
        }
    }
}
