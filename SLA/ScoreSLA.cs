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

        private void Awake()
        {
            TotalScoreText.text = HighScoreSLA.totalScoreSLA.ToString();
        }

        //count current and total score
        public void StartScore()
        {
            CurrentScore = -2;
            HighScoreSLA.totalScoreSLA -= 2;
            StartCoroutine(AddScore());
        }
    
        IEnumerator AddScore()
        {
            while (GameControl.dead == false)
            {
                CurrentScore += 2;
                HighScoreSLA.totalScoreSLA += 2;
                CurrentScoreText.text = CurrentScore.ToString();
                TotalScoreText.text = HighScoreSLA.totalScoreSLA.ToString();
            
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
        public void SetLevelHighScore()
        {
            if (CurrentScore > HighScoreSLA.highScoreSLA[GameControl.currentLevel])
            {
                NewHighScoreSLA();
                HighScoreSLA.highScoreSLA[GameControl.currentLevel] = CurrentScore;
                PlayerPrefs.SetInt("HighScoreSLA" + GameControl.currentLevel, HighScoreSLA.highScoreSLA[GameControl.currentLevel]);
            }
        }

        //compare total score to best game and set highscore
        public void SetGameHighScore()
        {
            if (HighScoreSLA.totalScoreSLA > HighScoreSLA.highScoreSLA[0])
            {
                HighScoreSLA.highScoreSLA[0] = HighScoreSLA.totalScoreSLA;
            }
            PlayerPrefs.SetInt("HighScoreSLAGame", HighScoreSLA.highScoreSLA[0]);
        }

        //add level highscores for combined score
        public void SetCombinedScore()
        {
            HighScoreSLA.highScoreSLA[14] = 0;
            for (var i = 1; i <= LevelManagerSLA.NumLevels; i++)
            {
                HighScoreSLA.highScoreSLA[14] += HighScoreSLA.highScoreSLA[i];
            }
            PlayerPrefs.SetInt("HighScoreSLACombined", HighScoreSLA.highScoreSLA[14]);
        }

    }
}