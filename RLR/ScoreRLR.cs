using System.Collections;
using System.Collections.Generic;
using Launcher;
using RLR.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RLR
{
    public class ScoreRLR : MonoBehaviour
    {
        public CheckSafeZones CheckSafeZones;
        public GameObject CurrentScoreText;
        public GameObject CountDownText;
        public Text NewHighScoreText;
        public GameObject NewHighScoreObject;
        public float[] FinishTimeCurGame = new float[LevelManagerRLR.NumLevels];

        private int _timeLimit;
        private float _countdown;
        private float _timer;
        private int _difficultyMultiplier;
        private float _initializationTime;

        private void Awake()
        {
            switch (GameControl.State.SetDifficulty)
            {
                case Difficulty.Normal:
                    _difficultyMultiplier = 1;
                    break;
                case Difficulty.Hard:
                    _difficultyMultiplier = 2;
                    break;
            }
        }

        public void StartTimer()
        {
            _timeLimit = 285 + 15 * GameControl.State.CurrentLevel;
            _initializationTime = Time.time;
            _countdown = _timeLimit;
        }

        private void Update()
        {
            if (_countdown > 0 && !GameControl.State.FinishedLevel && GameControl.State.GameActive)
            {
                _timer = Time.time - _initializationTime;
                if (GameControl.State.SetGameMode == Gamemode.TimeMode)
                {
                    _countdown = _timeLimit - _timer;
                    CountDownText.GetComponent<TextMeshProUGUI>().text = "Countdown: " + (int)_countdown / 60 + ":" + (_countdown % 60).ToString("00.00");
                }
            }
        }

        public void AddScore(GameObject currentSafeZone, List<GameObject> safeZones)
        {
            var index = CheckSafeZones.GetPlatformIndex(currentSafeZone);
            if (index == null) return;
            if (!GameControl.MapState.VisitedSafeZones[index.Value] && index.Value != 0)
            {
                if (GameControl.State.SetGameMode == Gamemode.TimeMode)
                {
                    GameControl.State.TotalScore += _difficultyMultiplier * GameControl.State.CurrentLevel;
                    CurrentScoreText.GetComponent<TextMeshProUGUI>().text = "Current Score: " + GameControl.State.TotalScore;
                    GameControl.MapState.VisitedSafeZones[index.Value] = true;
                    SetTimeModeHighScore();
                }
            }
        }

        public void AddRemainingCountdown()
        {
            GameControl.State.TotalScore += (int) _countdown;
            _countdown = 0;
        }

        //Checks for a new highscore and saves it
        public void SetHighScore()
        {
            if (GameControl.State.FinishedLevel && GameControl.State.SetGameMode != Gamemode.Practice)
            {
                FinishTimeCurGame[GameControl.State.CurrentLevel - 1] = _timer;

                switch (GameControl.State.SetDifficulty)
                {
                    case Difficulty.Normal:
                        if (_timer < GameControl.HighScores.HighScoreRLRNormal[GameControl.State.CurrentLevel] || GameControl.HighScores.HighScoreRLRNormal[GameControl.State.CurrentLevel] < 0.1f)
                        {
                            StartCoroutine(NewHighScore(_timer));
                            GameControl.HighScores.HighScoreRLRNormal[GameControl.State.CurrentLevel] = _timer;
                            PlayerPrefs.SetFloat("HighScoreRLRNormal" + GameControl.State.CurrentLevel,
                                GameControl.HighScores.HighScoreRLRNormal[GameControl.State.CurrentLevel]);
                        }
                        break;
                    case Difficulty.Hard:
                        if (_timer < GameControl.HighScores.HighScoreRLRHard[GameControl.State.CurrentLevel] || GameControl.HighScores.HighScoreRLRHard[GameControl.State.CurrentLevel] < 0.1f)
                        {
                            StartCoroutine(NewHighScore(_timer));
                            GameControl.HighScores.HighScoreRLRHard[GameControl.State.CurrentLevel] = _timer;
                            PlayerPrefs.SetFloat("HighScoreRLRHard" + GameControl.State.CurrentLevel,
                                GameControl.HighScores.HighScoreRLRHard[GameControl.State.CurrentLevel]);
                        }
                        break;
                }
            }
            if (GameControl.State.SetGameMode == Gamemode.TimeMode)
            {
                SetTimeModeHighScore();
            }
            PlayerPrefs.Save();
        }

        public void SetTimeModeHighScore()
        {

            switch (GameControl.State.SetDifficulty)
            {
                case Difficulty.Normal:
                    if (GameControl.State.TotalScore > GameControl.HighScores.HighScoreRLRNormal[0])
                    {
                        GameControl.HighScores.HighScoreRLRNormal[0] = GameControl.State.TotalScore;
                    }
                    PlayerPrefs.SetFloat("HighScoreRLRNormalTimeMode", GameControl.HighScores.HighScoreRLRNormal[0]);
                    break;
                case Difficulty.Hard:
                    if (GameControl.State.TotalScore >= GameControl.HighScores.HighScoreRLRHard[0])
                    {
                        GameControl.HighScores.HighScoreRLRHard[0] = GameControl.State.TotalScore;
                    }
                    PlayerPrefs.SetFloat("HighScoreRLRHardTimeMode", GameControl.HighScores.HighScoreRLRHard[0]);
                    break;
            }
        }

        private IEnumerator NewHighScore(float timer)
        {
            var record = timer;
            NewHighScoreText.text = record > 60 ? "New Highscore: " + (int)record / 60 + ":" + (record % 60).ToString("00.00") : "New Highscore: " + record.ToString("f2");
            NewHighScoreObject.SetActive(true);
            yield return new WaitForSeconds(3);
            NewHighScoreObject.SetActive(false);
        }
    }
}
