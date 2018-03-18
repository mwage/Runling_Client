using Client.Scripts.Launcher;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Players;
using Game.Scripts.SLA;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.SLA
{
    public class ScoreSLA : MonoBehaviour
    {
        public GameObject PlayerScorePrefab;
        public Transform ScoreLayoutGroup;
        public Text NewHighScore;

        public Dictionary<PlayerManager, ScoreDataSLA> Scores { get; } = new Dictionary<PlayerManager, ScoreDataSLA>();

        private ControlSLA _controlSLA;

        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
        }

        public void InitializeScore(PlayerManager playerManager)
        {
            var playerScore = Instantiate(PlayerScorePrefab, ScoreLayoutGroup);
            playerScore.transform.Find("PlayerName").GetComponent<Text>().text = playerManager.Player.Name;
            var currentScoreText = playerScore.transform.Find("CurrentScore").GetComponent<Text>();
            var totalScoreText = playerScore.transform.Find("TotalScore").GetComponent<Text>();
            Scores[playerManager] = new ScoreDataSLA(playerManager, LevelManagerSLA.NumLevels, currentScoreText, totalScoreText);
        }

        public void StartScore()
        {
            if (GameControl.GameState.Solo)
            {
                StartCoroutine(AddScore());
            }
        }

        private IEnumerator AddScore()
        {
            yield return new WaitForSeconds(0.25f);
            while (!_controlSLA.PlayerManagers[0].IsDead)
            {
                UpdateScore(_controlSLA.PlayerManagers[0]);
                yield return new WaitForSeconds (0.25f);
            }
        }

        private void UpdateScore(PlayerManager playerManager)
        {
            Scores[playerManager].IncrementScore(2);
        }

        public void SortScores()
        {
            var totalScores = Scores.Values.ToList();
            totalScores.Sort((score1, score2) => score2.TotalScore.CompareTo(score1.TotalScore));

            for (var i = 0; i < totalScores.Count; i++)
            {
                totalScores[i].SetSiblingIndex(i);
            }
        }

        public void SetHighScore(PlayerManager playerManager)
        {
            if (Scores[playerManager].CurrentScore > GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel])
            {
                NewHighScoreSLA(playerManager);
                GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel] = Scores[playerManager].CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + _controlSLA.CurrentLevel, GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel]);
            }

            SetGameHighScore(playerManager);
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        public void NewHighScoreSLA(PlayerManager playerManager)
        {
            NewHighScore.text = "New Highscore: " + Scores[playerManager].CurrentScore;
            NewHighScore.transform.parent.gameObject.SetActive(true);
        }

        public void SetGameHighScore(PlayerManager playerManager)
        {
            if (Scores[playerManager].TotalScore > GameControl.HighScores.HighScoreSLA[0])
            {
                GameControl.HighScores.HighScoreSLA[0] = Scores[playerManager].TotalScore;
            }
            PlayerPrefs.SetInt("HighScoreSLAGame", GameControl.HighScores.HighScoreSLA[0]);
        }

        public void SetCombinedScore()
        {
            GameControl.HighScores.HighScoreSLA[14] = 0;
            for (var i = 1; i <= LevelManagerSLA.NumLevels; i++)
            {
                GameControl.HighScores.HighScoreSLA[14] += GameControl.HighScores.HighScoreSLA[i];
            }
            PlayerPrefs.SetInt("HighScoreSLACombined", GameControl.HighScores.HighScoreSLA[14]);
        }
    }
}