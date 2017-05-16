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
            TotalScoreText.text = GameControl.Instance.State.TotalScore.ToString();
        }

        //count current and total score
        public void StartScore()
        {
            CurrentScore = -2;
            GameControl.Instance.State.TotalScore -= 2;
            StartCoroutine(AddScore());
        }
    
        IEnumerator AddScore()
        {
            while (GameControl.Instance.State.IsDead == false)
            {
                CurrentScore += 2;
                GameControl.Instance.State.TotalScore += 2;
                CurrentScoreText.text = CurrentScore.ToString();
                TotalScoreText.text = GameControl.Instance.State.TotalScore.ToString();
            
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
            LevelScoreCurGame[GameControl.Instance.State.CurrentLevel - 1] = CurrentScore;

            if (CurrentScore > GameControl.Instance.HighScores.HighScoreSLA[GameControl.Instance.State.CurrentLevel])
            {
                NewHighScoreSLA();
                GameControl.Instance.HighScores.HighScoreSLA[GameControl.Instance.State.CurrentLevel] = CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.Instance.State.CurrentLevel, GameControl.Instance.HighScores.HighScoreSLA[GameControl.Instance.State.CurrentLevel]);
            }

            SetGameHighScore();
            SetCombinedScore();
            PlayerPrefs.Save();
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (GameControl.Instance.State.TotalScore > GameControl.Instance.HighScores.HighScoreSLA[0])
            {
                GameControl.Instance.HighScores.HighScoreSLA[0] = GameControl.Instance.State.TotalScore;
            }
            PlayerPrefs.SetInt("HighScoreSLAGame", GameControl.Instance.HighScores.HighScoreSLA[0]);
        }

        //add level highscores for combined score
        public void SetCombinedScore()
        {
            GameControl.Instance.HighScores.HighScoreSLA[14] = 0;
            for (var i = 1; i <= LevelManagerSLA.NumLevels; i++)
            {
                GameControl.Instance.HighScores.HighScoreSLA[14] += GameControl.Instance.HighScores.HighScoreSLA[i];
            }
            PlayerPrefs.SetInt("HighScoreSLACombined", GameControl.Instance.HighScores.HighScoreSLA[14]);
        }
    }
}