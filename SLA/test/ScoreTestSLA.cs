using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SLA.test
{
    public class ScoreTestSLA : MonoBehaviour
    {

        //attach scripts
        public int currentScore;
        public Text currentScoreText;
        public Text totalScoreText;

        private void Awake()
        {
            totalScoreText.text = 0.ToString();
            currentScoreText.text = 0.ToString();
        }

        //count current and total score
        public void StartScore()
        {
            currentScore = -2;
            StartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            while (GameControl.dead == false)
            {
                currentScore += 2;
                currentScoreText.text = currentScore.ToString();
                totalScoreText.text = currentScore.ToString();

                yield return new WaitForSeconds(0.5f);
            }
        }    
    }
}
