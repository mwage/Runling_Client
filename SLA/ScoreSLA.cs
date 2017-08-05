using System.Collections;
using Launcher;
using Players;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class ScoreSLA : MonoBehaviour
    {
        private ControlSLA _controlSLA;
        public GameObject PlayerScorePrefab;
        public Transform ScoreLayoutGroup;
        public Text NewHighScore;

        public int[] LevelScoreCurGame = new int[LevelManagerSLA.NumLevels];
        private GameObject _playerScores;
        public Text CurrentScoreText;
        private Text _totalScoreText;
        private Text _playerNameText;

        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLA>();
        }

        private void Start()
        {
                _playerScores = Instantiate(PlayerScorePrefab, ScoreLayoutGroup);
                _playerNameText = _playerScores.transform.Find("PlayerName").GetComponent<Text>();
                CurrentScoreText = _playerScores.transform.Find("CurrentScore").GetComponent<Text>();
                _totalScoreText = _playerScores.transform.Find("TotalScore").GetComponent<Text>();

//                _playerNameText.text = PhotonNetwork.player.NickName;
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
            LevelScoreCurGame[GameControl.GameState.CurrentLevel - 1] = playerManager.CurrentScore;

            if (playerManager.CurrentScore > GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel])
            {
                NewHighScoreSLA(playerManager);
                GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel] = playerManager.CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.GameState.CurrentLevel, GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel]);
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