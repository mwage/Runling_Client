using System.Collections;
using System.Collections.Generic;
using Launcher;
using RLR.Levels;
using TMPro;
using UnityEngine;

namespace RLR
{
    public class ScoreRLR : MonoBehaviour
    {
        public CheckSafeZones CheckSafeZones;
        public GameObject CurrentScoreText;
        public GameObject CountDownText;
        public int Score;

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
                _countdown = _timeLimit - _timer;
                CountDownText.GetComponent<TextMeshProUGUI>().text ="Countdown: " + (int) _countdown / 60 + ":" + (_countdown % 60).ToString("f2");
            }
        }

        public void AddScore(GameObject currentSafeZone, List<GameObject> safeZones)
        {
            var index = CheckSafeZones.GetPlatformIndex(currentSafeZone, safeZones);
            if (index == null) return;
            if (!CheckSafeZones.VisitedSafeZone[index.Value] && index.Value != 0)
            {
                Score += _difficultyMultiplier * GameControl.State.CurrentLevel;
                CurrentScoreText.GetComponent<TextMeshProUGUI>().text = "Current Score: " + Score;
                CheckSafeZones.VisitedSafeZone[index.Value] = true;
            }
        }

        public void AddRemainingCountdown()
        {
            Score += (int) _countdown;
            _countdown = 0;
        }

        public void SetHighScores()
        {
            
        }
    }
}
