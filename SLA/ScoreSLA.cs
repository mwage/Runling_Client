using System.Collections;
using Launcher;
using SLA.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class ScoreSLA : MonoBehaviour
    {
        public GameObject PlayerScorePrefab;
        public Transform ScoreLayoutGroup;
        public Text NewHighScore;

        public int[] LevelScoreCurGame = new int[LevelManagerSLA.NumLevels];
        private GameObject[] _playerScores;
        public Text[] CurrentScoreText;
        private Text[] _totalScoreText;
        private Text[] _playerNameText;
        private PhotonView _photonView;


        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _playerScores = new GameObject[PhotonNetwork.room.PlayerCount];
            CurrentScoreText = new Text[PhotonNetwork.room.PlayerCount];
            _totalScoreText = new Text[PhotonNetwork.room.PlayerCount];
            _playerNameText = new Text[PhotonNetwork.room.PlayerCount];

            foreach (var state in GameControl.PlayerState.SyncVars)
            {
                if (state == null)
                    continue;

                _playerScores[state.Owner.ID - 1] = Instantiate(PlayerScorePrefab, ScoreLayoutGroup);
                _playerNameText[state.Owner.ID - 1] = _playerScores[state.Owner.ID - 1].transform.Find("PlayerName").GetComponent<Text>();
                CurrentScoreText[state.Owner.ID - 1] = _playerScores[state.Owner.ID - 1].transform.Find("CurrentScore").GetComponent<Text>();
                _totalScoreText[state.Owner.ID - 1] = _playerScores[state.Owner.ID - 1].transform.Find("TotalScore").GetComponent<Text>();

                _playerNameText[state.Owner.ID - 1].text = state.Owner.NickName;
                _totalScoreText[state.Owner.ID - 1].text = state.TotalScore.ToString();
            }
        }

        //count current and total score
        public void StartScore()
        {
            foreach (var state in GameControl.PlayerState.SyncVars)
            {
                if (state != null)
                    state.CurrentScore = 0;
            }

            foreach (var text in CurrentScoreText)
            {
                text.text = "0";
            }

            StartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            while (!GameControl.GameState.AllDead)
            {
                yield return new WaitForSeconds(0.25f);

                if (PhotonNetwork.isMasterClient)
                    _photonView.RPC("UpdateScore", PhotonTargets.All);
            }
        }

        [PunRPC]
        private void UpdateScore()
        {
            foreach (var state in GameControl.PlayerState.SyncVars)
            {
                if (state == null || state.IsDead)
                    continue;

                state.CurrentScore += 2;
                state.TotalScore += 2;
                CurrentScoreText[state.Owner.ID - 1].text = state.CurrentScore.ToString();
                _totalScoreText[state.Owner.ID - 1].text = state.TotalScore.ToString();
            }
        }

        //message that you got a new highscore
        public void NewHighScoreSLA()
        {
            NewHighScore.text = "New Highscore: " + GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].CurrentScore;
            NewHighScore.transform.parent.gameObject.SetActive(true);
        }

        //Checks for a new highscore and saves it
        public void SetHighScore()
        {
            LevelScoreCurGame[GameControl.GameState.CurrentLevel - 1] = GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].CurrentScore;

            if (GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].CurrentScore > GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel])
            {
                NewHighScoreSLA();
                GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel] = GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.GameState.CurrentLevel, GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel]);
            }

            SetGameHighScore();
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].TotalScore > GameControl.HighScores.HighScoreSLA[0])
            {
                GameControl.HighScores.HighScoreSLA[0] = GameControl.PlayerState.SyncVars[PhotonNetwork.player.ID - 1].TotalScore;
            }
            PlayerPrefs.SetInt("HighScoreSLAGame", GameControl.HighScores.HighScoreSLA[0]);
        }

        //add level highscores for combined score
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