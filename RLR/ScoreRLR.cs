using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RLR
{
    public class ScoreRLR : MonoBehaviour
    {
        //attach gameobjects
        public GameObject HighScore;

        //attach scripts
        public int CurrentScore;
        public Text CurrentScoreText;
        public Text TotalScoreText;
        public Text NewHighScore;
        public int[] LevelScoreCurGame = new int[LevelManagerRLR.NumLevels];

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
        public void NewHighScoreRLR()
        {
            NewHighScore.text = "New Highscore: " + CurrentScore.ToString();
            HighScore.SetActive(true);
        }

        //Checks for a new highscore and saves it
        public void SetHighScore()
        {
            Debug.Log(GameControl.CurrentLevel);
            LevelScoreCurGame[GameControl.CurrentLevel - 1] = CurrentScore;

            if (CurrentScore > HighScoreRLR.highScoreRLR[GameControl.CurrentLevel])
            {
                NewHighScoreRLR();
                HighScoreRLR.highScoreRLR[GameControl.CurrentLevel] = CurrentScore;
                PlayerPrefs.SetInt("HighScoreRLR" + GameControl.CurrentLevel, HighScoreRLR.highScoreRLR[GameControl.CurrentLevel]);
            }

            SetGameHighScore();
            SetCombinedScore();
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (GameControl.TotalScore > HighScoreRLR.highScoreRLR[0])
            {
                HighScoreRLR.highScoreRLR[0] = GameControl.TotalScore;
            }
            PlayerPrefs.SetInt("HighScoreRLRGame", HighScoreRLR.highScoreRLR[0]);
        }

        //add level highscores for combined score
        public void SetCombinedScore()
        {
            HighScoreRLR.highScoreRLR[14] = 0;
            for (var i = 1; i <= LevelManagerRLR.NumLevels; i++)
            {
                HighScoreRLR.highScoreRLR[14] += HighScoreRLR.highScoreRLR[i];
            }
            PlayerPrefs.SetInt("HighScoreRLRCombined", HighScoreRLR.highScoreRLR[14]);
        }
    }
}