using System.Collections;
using Launcher;
using SLA.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace SLA
{
    public class ScoreSLA : MonoBehaviour
    {
        //attach gameobjects
        public GameObject HighScore;

        //attach scripts
        public int CurrentScore;
        public Text CurrentScoreText;
        public Text TotalScoreText;
        public Text NewHighScore;
        public int[] LevelScoreCurGame = new int[LevelManagerSLA.NumLevels];

        private void Awake()
        {
            TotalScoreText.text = GameControl.State.TotalScore.ToString();
        }

        //count current and total score
        public void StartScore()
        {
            CurrentScore = -2;
            GameControl.State.TotalScore -= 2;
            StartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            while (GameControl.State.IsDead == false)
            {
                CurrentScore += 2;
                GameControl.State.TotalScore += 2;
                CurrentScoreText.text = CurrentScore.ToString();
                TotalScoreText.text = GameControl.State.TotalScore.ToString();
            
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
            LevelScoreCurGame[GameControl.State.CurrentLevel - 1] = CurrentScore;

            if (CurrentScore > GameControl.HighScores.HighScoreSLA[GameControl.State.CurrentLevel])
            {
                NewHighScoreSLA();
                GameControl.HighScores.HighScoreSLA[GameControl.State.CurrentLevel] = CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.State.CurrentLevel, GameControl.HighScores.HighScoreSLA[GameControl.State.CurrentLevel]);
            }

            SetGameHighScore();
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (GameControl.State.TotalScore > GameControl.HighScores.HighScoreSLA[0])
            {
                GameControl.HighScores.HighScoreSLA[0] = GameControl.State.TotalScore;
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