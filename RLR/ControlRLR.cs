using Launcher;
using Network.Synchronization.Data;
using Players;
using System.Collections;
using Characters.Repositories;
using TMPro;
using UnityEngine;

namespace RLR
{
    public class ControlRLR : MonoBehaviour
    {
        public GameObject PracticeMode;
        public GameObject TimeModeUI;
        public GameObject CountDownText;
        public GameObject HighScoreText;

        public PlayerManager PlayerManager { get; private set; }
        public int CurrentLevel { get; set; } = 1;

        private LevelManagerRLR _levelManager;
        private InitializeGameRLR _initializeGame;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerRLR>();
            _initializeGame = GetComponent<InitializeGameRLR>();

            PlayerTrigger.onFinishedLevel += OnFinishedLevel;
        }

        private void OnDestroy()
        {
            PlayerTrigger.onFinishedLevel -= OnFinishedLevel;
        }

        private void Start()
        {
            // temporary for testing
            if (GameControl.GameState.CharacterDto == null)
            {
                GameControl.GameState.CharacterDto = new CharacterDto(9, "Cat", 50, 50, 50, 0, 0, 1, 1);
                GameControl.GameState.Solo = true;
            }

            // Set gamemode specific UI
            if (GameControl.GameState.SetGameMode == GameMode.Practice)
            {
                PracticeMode.SetActive(true);
            }
            else if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
              
                TimeModeUI.SetActive(true);

                CountDownText.GetComponent<TextMeshProUGUI>().text = 
                    "Countdown: " + (285 + CurrentLevel*15) / 60 + ":" + 
                    ((285 + CurrentLevel*15) % 60).ToString("00.00");

                HighScoreText.GetComponent<TextMeshProUGUI>().text = GameControl.GameState.SetDifficulty == 
                    Difficulty.Normal ? "Highscore: " + GameControl.HighScores.HighScoreRLRNormal[0].ToString("f0") : 
                    "Highscore: " + GameControl.HighScores.HighScoreRLRHard[0].ToString("f0");
            }

            // Only initialize on it's own when playing solo
            if (!GameControl.GameState.Solo)
                return;

            PlayerManager = _initializeGame.InitializePlayer(new Player(0, PlayerPrefs.GetString("username"), PlayerColor.Green));
            _initializeGame.InitializeControls(PlayerManager);
            _initializeGame.InitializeSafeZones(PlayerManager);
            PlayerManager.PlayerMovement = PlayerManager.gameObject.AddComponent<PlayerMovement>();
            
            if (GameControl.GameState.SetGameMode == GameMode.TimeMode)
            {
                _initializeGame.ChangeLives(PlayerManager, 3);
            }



            InitializeLevel();
        }

        //set Spawnimmunity once game starts
        public void InitializeLevel()
        {
            GameControl.GameState.FinishedLevel = false;
            StartCoroutine(PrepareLevel());
        }

        private IEnumerator PrepareLevel()
        {
            // Show Highscore and current level
            _initializeGame.PrepareLevel();
            _initializeGame.SpawnPlayer(PlayerManager);
            _initializeGame.SetCameraPosition(PlayerManager);
            _levelManager.LoadDrones(CurrentLevel);

            yield return new WaitForSeconds(2);
            _initializeGame.HideText();
            yield return new WaitForSeconds(1);

            // Countdown
            for (var i = 3; i >= 0; i--)
            {
                _initializeGame.Countdown(i);

                if (i != 0)
                {
                    yield return new WaitForSeconds(1);
                }
            }

            _initializeGame.StartLevel(PlayerManager);
        }

        public void OnFinishedLevel()
        {
            _levelManager.EndLevel(0);
        }
    }
}