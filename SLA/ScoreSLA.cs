using System.Collections;
using Launcher;
using Players;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class ScoreSLA : MonoBehaviour
    {
        public GameObject PlayerScorePrefab;
        public Transform ScoreLayoutGroup;
        public Text NewHighScore;

        public int[] LevelScoreCurGame { get; } = new int[LevelManagerSLA.NumLevels];
        public Text CurrentScoreText { get; private set; }

        private ControlSLA _controlSLA;
        private GameObject _playerScores;
        private Text _totalScoreText;

        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
        }

        public void SetName(string playerName)
        {
            _playerScores = Instantiate(PlayerScorePrefab, ScoreLayoutGroup);
            _playerScores.transform.Find("PlayerName").GetComponent<Text>().text = playerName;
            CurrentScoreText = _playerScores.transform.Find("CurrentScore").GetComponent<Text>();
            _totalScoreText = _playerScores.transform.Find("TotalScore").GetComponent<Text>();

            CurrentScoreText.text = "0";
            _totalScoreText.text = "0";
        }

        public void StartScore()
        {
            _controlSLA.PlayerManager.CurrentScore = 0;
            StartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            while (!_controlSLA.PlayerManager.IsDead)
            {
                yield return new WaitForSeconds (0.25f);
                UpdateScore(_controlSLA.PlayerManager);
            }
        }

        private void UpdateScore(PlayerManager playerManager)
        {
                playerManager.CurrentScore += 2;
                playerManager.TotalScore += 2;
                CurrentScoreText.text = playerManager.CurrentScore.ToString();
                _totalScoreText.text = playerManager.TotalScore.ToString();
        }


        public void SetHighScore()
        {
            var playerManager = _controlSLA.PlayerManager;
            LevelScoreCurGame[_controlSLA.CurrentLevel - 1] = playerManager.CurrentScore;

            if (playerManager.CurrentScore > GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel])
            {
                NewHighScoreSLA(playerManager);
                GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel] = playerManager.CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + _controlSLA.CurrentLevel, GameControl.HighScores.HighScoreSLA[_controlSLA.CurrentLevel]);
            }

            SetGameHighScore(playerManager);
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        public void NewHighScoreSLA(PlayerManager playerManager)
        {
            NewHighScore.text = "New Highscore: " + playerManager.CurrentScore;
            NewHighScore.transform.parent.gameObject.SetActive(true);
        }

        public void SetGameHighScore(PlayerManager playerManager)
        {
            if (playerManager.TotalScore > GameControl.HighScores.HighScoreSLA[0])
            {
                GameControl.HighScores.HighScoreSLA[0] = playerManager.TotalScore;
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