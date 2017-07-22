using System.Collections;
using Launcher;
using SLA.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class ScoreSLA : MonoBehaviour
    {
        public NetworkManagerSLA NetworkManager;
        public GameObject HighScore;
        public int CurrentScore;
        public Text CurrentScoreText;
        public Text TotalScoreText;
        public Text NewHighScore;
        public int[] LevelScoreCurGame = new int[LevelManagerSLA.NumLevels];

        private void Awake()
        {
            TotalScoreText.text = NetworkManager.SyncVars[PhotonNetwork.player.ID - 1].TotalScore.ToString();
        }

        //count current and total score
        public void StartScore()
        {
            CurrentScore = -2;
            NetworkManager.SyncVars[PhotonNetwork.player.ID - 1].TotalScore -= 2;
            StartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            while (NetworkManager.SyncVars[PhotonNetwork.player.ID - 1].IsDead == false)
            {
                CurrentScore += 2;
                NetworkManager.SyncVars[PhotonNetwork.player.ID - 1].TotalScore += 2;
                CurrentScoreText.text = CurrentScore.ToString();
                TotalScoreText.text = NetworkManager.SyncVars[PhotonNetwork.player.ID - 1].TotalScore.ToString();
            
                yield return new WaitForSeconds(0.25f);
            }
        }

        //message that you got a new highscore
        public void NewHighScoreSLA()
        {
            NewHighScore.text = "New Highscore: " + CurrentScore;
            HighScore.SetActive(true);
        }

        //Checks for a new highscore and saves it
        public void SetHighScore()
        {
            LevelScoreCurGame[GameControl.GameState.CurrentLevel - 1] = CurrentScore;

            if (CurrentScore > GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel])
            {
                NewHighScoreSLA();
                GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel] = CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.GameState.CurrentLevel, GameControl.HighScores.HighScoreSLA[GameControl.GameState.CurrentLevel]);
            }

            SetGameHighScore();
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (GameControl.PlayerState.TotalScore > GameControl.HighScores.HighScoreSLA[0])
            {
                GameControl.HighScores.HighScoreSLA[0] = GameControl.PlayerState.TotalScore;
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