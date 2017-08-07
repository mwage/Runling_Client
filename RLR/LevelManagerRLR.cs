using System;
using System.Collections;
using System.Collections.Generic;
using Drones;
using Launcher;
using Players;
using RLR.GenerateMap;
using RLR.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RLR
{
    public class LevelManagerRLR : MonoBehaviour
    {
        public DroneFactory DroneFactory;
        public MapGeneratorRLR MapGenerator;
        public RunlingChaser RunlingChaser;
        public GameObject Win;
        public GameObject LivesText;

        private InitializeGameRLR _initializeGame;
        private ScoreRLR _score;
        private ControlRLR _controlRLR;
        private readonly InitializeLevelsRLR _initializeLevelsRLR = new InitializeLevelsRLR();

        public static int NumLevels = 9;             //currently last level available in RLR
        private List<ILevelRLR> _levels;


        public void Awake()
        {
            _initializeGame = GetComponent<InitializeGameRLR>();
            _score = GetComponent<ScoreRLR>();
            _controlRLR = GetComponent<ControlRLR>();

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
            StartCoroutine(GameControl.GameState.CurrentLevel == _levels.Count ? EndGameRLR(delay) : LoadNextLevel(0));
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

            // Destroy all enemies and stop all pattern
            DroneFactory.StopAllCoroutines();
            foreach (Transform child in DroneFactory.transform)
            {
                Destroy(child.gameObject);
            }

            if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
                _score.AddRemainingCountdown();
                _score.CurrentScoreText.GetComponent<TextMeshProUGUI>().text = "Current Score: " +_controlRLR.PlayerManager.TotalScore;
                _controlRLR.PlayerManager.Lives = 3;
                LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + _controlRLR.PlayerManager.Lives;
            }
            if (GameControl.GameState.SetGameMode != GameMode.Practice)
            {
               _score.SetHighScore();
            }
            GameControl.GameState.FinishedLevel = false;
            GameControl.GameState.CurrentLevel++;
            _initializeGame.InitializeGame();
        }

        // Load after the last level
        private IEnumerator EndGameRLR(float delay)
        {
            if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
               _score.AddRemainingCountdown();
                _score.CurrentScoreText.GetComponent<TextMeshProUGUI>().text = "Current Score: " + _controlRLR.PlayerManager.TotalScore;
            }

            if (!_controlRLR.PlayerManager.IsDead)
            {
                Win.transform.Find("Victory").gameObject.SetActive(true);
            }
            else
            {
                Win.transform.Find("Defeat").gameObject.SetActive(true);
            }

            // Load win screen
            yield return new WaitForSeconds(delay);
            _initializeGame.InGameMenuManager.CloseMenus();
            if (GameControl.GameState.SetGameMode != GameMode.Practice)
            {
                _score.SetHighScore();
            }
            GameControl.GameState.FinishedLevel = false;
            _controlRLR.PlayerManager.gameObject.SetActive(false);
            Win.gameObject.SetActive(true);
        }
    }
}
