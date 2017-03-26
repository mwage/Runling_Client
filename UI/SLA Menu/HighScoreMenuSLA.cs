using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreMenuSLA : MonoBehaviour {

    public GameObject menu;

    public bool highScoreMenuActive;

    public Text hSTextSLA1;
    public Text hSTextSLA2;
    public Text hSTextSLA3;
    public Text hSTextSLA4;
    public Text hSTextSLA5;

    public Text hSTextSLAGame;
    public Text hSTextSLACombined;


    public void Awake()
    {
        hSTextSLA1.text = HighScoreSLA.highScoreSLA[1].ToString();
        hSTextSLA2.text = HighScoreSLA.highScoreSLA[2].ToString();
        hSTextSLA3.text = HighScoreSLA.highScoreSLA[3].ToString();
        hSTextSLA4.text = HighScoreSLA.highScoreSLA[4].ToString();
        hSTextSLA5.text = HighScoreSLA.highScoreSLA[5].ToString();

        hSTextSLAGame.text = HighScoreSLA.highScoreSLA[0].ToString();
        hSTextSLACombined.text = HighScoreSLA.highScoreSLA[14].ToString();
    }

    public void Back()
    {
        highScoreMenuActive = false;
        gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }
}
