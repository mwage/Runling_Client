using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SLAMenu : MonoBehaviour {

    public HighScoreMenuSLA _highScoreMenuSLA;

    public GameObject mainMenu;
    public GameObject highScoreMenu;

    public bool SLAMenuActive;

    public void StartGame()
    {
        GameControl.gameActive = true;
        GameControl.dead = true;
        HighScoreSLA.totalScoreSLA = 0;
        GameControl.currentLevel = 0;

        SceneManager.LoadScene("SLA");
    }

    public void HighScores()
    {
        gameObject.SetActive(false);
        SLAMenuActive = false;
        highScoreMenu.gameObject.SetActive(true);
        _highScoreMenuSLA.highScoreMenuActive = true;
    }

    public void BackToMenu()
    {
        SLAMenuActive = false;
        gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
