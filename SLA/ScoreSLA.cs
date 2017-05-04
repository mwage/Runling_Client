using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SLA
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
            TotalScoreText.text = GameControl.TotalScore.ToString();
        }

        //count current and total score
        public void StartScore()
        {
            CurrentScore = -2;
            GameControl.TotalScore -= 2;
            StartCoroutine(AddScore());
        }
    
        IEnumerator AddScore()
        {
            while (GameControl.Dead == false)
            {
                CurrentScore += 2;
                GameControl.TotalScore += 2;
                CurrentScoreText.text = CurrentScore.ToString();
                TotalScoreText.text = GameControl.TotalScore.ToString();
            
                yield return new WaitForSeconds(0.25f);
            }
        }

        //message that you got a new highscore
        public void NewHighScoreSLA()
        {
            NewHighScore.text = "New Highscore: " + CurrentScore.ToString();
            HighScore.SetActive(true);
        }

        //Checks for a new highscore and saves it
        public void SetHighScore()
        {
            LevelScoreCurGame[GameControl.CurrentLevel - 1] = CurrentScore;

            if (CurrentScore > HighScores.highScoreSLA[GameControl.CurrentLevel])
            {
                NewHighScoreSLA();
                HighScores.highScoreSLA[GameControl.CurrentLevel] = CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.CurrentLevel, HighScores.highScoreSLA[GameControl.CurrentLevel]);
            }

            SetGameHighScore();
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (GameControl.TotalScore > HighScores.highScoreSLA[0])
            {
                HighScores.highScoreSLA[0] = GameControl.TotalScore;
            }
            PlayerPrefs.SetInt("HighScoreSLAGame", HighScores.highScoreSLA[0]);
        }

        //add level highscores for combined score
        public void SetCombinedScore()
        {
            HighScores.highScoreSLA[14] = 0;
            for (var i = 1; i <= LevelManagerSLA.NumLevels; i++)
            {
                HighScores.highScoreSLA[14] += HighScores.highScoreSLA[i];
            }
            PlayerPrefs.SetInt("HighScoreSLACombined", HighScores.highScoreSLA[14]);
        }
    }
}