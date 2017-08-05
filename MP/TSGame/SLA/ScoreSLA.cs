using System.Collections;
using Launcher;
using MP.TSGame.Players;
using TrueSync;
using UnityEngine;
using UnityEngine.UI;

namespace MP.TSGame.SLA
{
    public class ScoreSLA : TrueSyncBehaviour
    {
        public ControlSLA ControlSLA;
        public GameObject PlayerScorePrefab;
        public Transform ScoreLayoutGroup;
        public Text NewHighScore;

        public int[] LevelScoreCurGame = new int[LevelManagerSLA.NumLevels];
        private GameObject[] _playerScores;
        public Text[] CurrentScoreText;
        private Text[] _totalScoreText;
        private Text[] _playerNameText;


        private void Awake()
        {
            _playerScores = new GameObject[PhotonNetwork.room.PlayerCount];
            CurrentScoreText = new Text[PhotonNetwork.room.PlayerCount];
            _totalScoreText = new Text[PhotonNetwork.room.PlayerCount];
            _playerNameText = new Text[PhotonNetwork.room.PlayerCount];
        }

        public override void OnSyncedStart()
        {
            foreach (var player in TrueSyncManager.Players)
            {
                _playerScores[player.Id - 1] = Instantiate(PlayerScorePrefab, ScoreLayoutGroup);
                _playerNameText[player.Id - 1] = _playerScores[player.Id - 1].transform.Find("PlayerName").GetComponent<Text>();
                CurrentScoreText[player.Id - 1] = _playerScores[player.Id - 1].transform.Find("CurrentScore").GetComponent<Text>();
                _totalScoreText[player.Id - 1] = _playerScores[player.Id - 1].transform.Find("TotalScore").GetComponent<Text>();

                _playerNameText[player.Id - 1].text = player.Name;
                CurrentScoreText[player.Id - 1].text = "0";
                _totalScoreText[player.Id - 1].text = "0";
            }
        }

        public void StartScore()
        {
            foreach (var playerManager in ControlSLA.PlayerManager)
            {
                playerManager.CurrentScore = 0;
            }

            TrueSyncManager.SyncedStartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            while (!ControlSLA.AllDead)
            {
                yield return 0.25f;
                UpdateScore();
            }
        }

        private void UpdateScore()
        {
            foreach (var playerManager in ControlSLA.PlayerManager)
            {
                if (playerManager == null || playerManager.IsDead)
                    continue;

                playerManager.CurrentScore += 2;
                playerManager.TotalScore += 2;
                CurrentScoreText[playerManager.PlayerMovement.ownerIndex - 1].text = playerManager.CurrentScore.ToString();
                _totalScoreText[playerManager.PlayerMovement.ownerIndex - 1].text = playerManager.TotalScore.ToString();
            }
        }

        public void NewHighScoreSLA(PlayerManager playerManager)
        {
            NewHighScore.text = "New Highscore: " + playerManager.CurrentScore;
            NewHighScore.transform.parent.gameObject.SetActive(true);
        }

        public void SetHighScore()
        {
            var localPlayerManager = ControlSLA.PlayerManager[TrueSyncManager.LocalPlayer.Id - 1];
            LevelScoreCurGame[GameControl.GameState.CurrentLevel - 1] = localPlayerManager.CurrentScore;

            if (localPlayerManager.CurrentScore > GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel])
            {
                NewHighScoreSLA(localPlayerManager);
                GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel] = localPlayerManager.CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.GameState.CurrentLevel, GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel]);
            }

            SetGameHighScore(localPlayerManager);
            SetCombinedScore();
            PlayerPrefs.Save();
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