using System;
using System.Collections;
using System.Collections.Generic;
using Drones;
using Launcher;
using RLR.GenerateMap;
using TMPro;
using UI.RLR_Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RLR.Levels
{
    public class LevelManagerRLR : MonoBehaviour
    {

        //attach scripts
        public InGameMenuManagerRLR InGameMenuManagerRLR;
        public InitializeGameRLR InitializeGameRLR;
        public RunlingChaser RunlingChaser;
        public CheckSafeZones CheckSafeZones;
        public GameObject Win;
        public DroneFactory DroneFactory;
        public MapGeneratorRLR MapGeneratorRlr;

        public GameObject LivesText;

        private readonly InitializeLevelsRLR _initializeLevelsRLR = new InitializeLevelsRLR();

        public static int NumLevels = 9;             //currently last level available in RLR
        private List<ILevelRLR> _levels;


        public void Awake()
        {
            //GameControl.SetDifficulty = GameControl.Difficulty.Hard;
            _levels = _initializeLevelsRLR.SetDifficulty(this);
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

        public void GenerateMap(int level)
        {
            _levels[level - 1].GenerateMap();
        }

        public void GenerateChasers(int level)
        {
            _levels[level - 1].SetChasers();
        }

        // Load next level
        public void EndLevel(float delay)
        {
            StartCoroutine((GameControl.State.CurrentLevel == _levels.Count) ? EndGameRLR(delay) : LoadNextLevel(0));
        }

        // End game
        public void EndGame(float delay)
        {
            StartCoroutine(EndGameRLR(delay));
        }

        // Load in all but the last level
        private IEnumerator LoadNextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            DroneFactory.StopAllCoroutines();
            // Destroy(GameControl.PlayerState.Player); // dont destroy player. if exists playerfactory donest make new one, just move him on start position
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            var strongEnemies = GameObject.FindGameObjectsWithTag("Strong Enemy");
            foreach (var t in enemies)
            {
                Destroy(t);
            }
            foreach (var t in strongEnemies)
            {
                Destroy(t);
            }

            if (GameControl.State.SetGameMode == Gamemode.TimeMode)
            {
                CheckSafeZones.ScoreRLR.AddRemainingCountdown();
                CheckSafeZones.ScoreRLR.CurrentScoreText.GetComponent<TextMeshProUGUI>().text = "Current Score: " + GameControl.State.TotalScore;
                GameControl.PlayerState.Lives = 3;
                LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + GameControl.PlayerState.Lives;
            }
            if (GameControl.State.SetGameMode != Gamemode.Practice)
            {
                CheckSafeZones.ScoreRLR.SetHighScore();
            }
            GameControl.State.FinishedLevel = false;
            GameControl.State.CurrentLevel++;
            InitializeGameRLR.InitializeGame();
        }

        // Load after the last level
        private IEnumerator EndGameRLR(float delay)
        {
            if (GameControl.State.SetGameMode == Gamemode.TimeMode)
            {
                CheckSafeZones.ScoreRLR.AddRemainingCountdown();
                CheckSafeZones.ScoreRLR.CurrentScoreText.GetComponent<TextMeshProUGUI>().text = "Current Score: " + GameControl.State.TotalScore;
            }

            if (!GameControl.PlayerState.IsDead)
            {
                Win.transform.Find("Victory").gameObject.SetActive(true);
                Win.transform.Find("Defeat").gameObject.SetActive(false);
            }
            else
            {
                Win.transform.Find("Victory").gameObject.SetActive(false);
                Win.transform.Find("Defeat").gameObject.SetActive(true);
            }

            // Load win screen
            yield return new WaitForSeconds(delay);
            InGameMenuManagerRLR.CloseMenus();
            if (GameControl.State.SetGameMode != Gamemode.Practice)
            {
                CheckSafeZones.ScoreRLR.SetHighScore();
            }
            GameControl.State.FinishedLevel = false;
            GameControl.PlayerState.Player.SetActive(false);
            Win.gameObject.SetActive(true);
        }
    }
}
