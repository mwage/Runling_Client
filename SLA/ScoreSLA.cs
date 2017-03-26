using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSLA : MonoBehaviour
{
    //attach gameobjects
    public GameObject highScore;

    //attach scripts
    public int currentScore;
    public Text currentScoreText;
    public Text totalScoreText;
    public Text newHighScore;
    private void Awake()
    {
        totalScoreText.text = HighScoreSLA.totalScoreSLA.ToString();
    }

    //count current and total score
    public void StartScore()
    {
        currentScore = -2;
        HighScoreSLA.totalScoreSLA -= 2;
        StartCoroutine(AddScore());
    }
    
    IEnumerator AddScore()
    {
        while (GameControl.dead == false)
        {
            currentScore += 2;
            HighScoreSLA.totalScoreSLA += 2;
            currentScoreText.text = currentScore.ToString();
            totalScoreText.text = HighScoreSLA.totalScoreSLA.ToString();
            
            yield return new WaitForSeconds(0.25f);
        }
    }

    //message that you got a new highscore
    public void NewHighScoreSLA()
    {
        newHighScore.text = "New Highscore: " + currentScore.ToString();
        highScore.SetActive(true);
    }

    //Checks for a new highscore and saves it
    public void SetLevelHighScore()
    {
        if (currentScore > HighScoreSLA.highScoreSLA[GameControl.currentLevel])
        {
            NewHighScoreSLA();
            HighScoreSLA.highScoreSLA[GameControl.currentLevel] = currentScore;
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
        for (int i = 1; i <= GameControl.lastLevelSLA; i++)
        {
            HighScoreSLA.highScoreSLA[14] += HighScoreSLA.highScoreSLA[i];
        }
        PlayerPrefs.SetInt("HighScoreSLACombined", HighScoreSLA.highScoreSLA[14]);
    }

}