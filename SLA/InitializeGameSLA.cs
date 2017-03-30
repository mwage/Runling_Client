using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitializeGameSLA : MonoBehaviour {

    // Attach scripts
    public LevelManagerSLA _levelManagerSLA;
    public ScoreSLA _scoreSLA;
    public ControlSLA _controlSLA;
    public InGameMenuManagerSLA _inGameMenuManagerSLA;

    public GameObject player;
    public GameObject levelTextObject;
    public GameObject text3;
    public GameObject text2;
    public GameObject text1;
    public GameObject currentPRWindow;
    public GameObject newPlayer;
    public Text currentPR;
    TextMeshProUGUI levelText;


    //set Spawnimmunity once game starts
    public void InitializeGame()
    {
        StartCoroutine(PrepareLevel());
    }

    IEnumerator PrepareLevel()
    {
        // Set current Level and movespeed
        GameControl.moveSpeed = _levelManagerSLA.moveSpeedSLA[GameControl.currentLevel];
        GameControl.currentLevel++;


        // Show level highscore and current level
        currentPR.text = HighScoreSLA.highScoreSLA[GameControl.currentLevel].ToString();
        levelText = levelTextObject.GetComponent<TextMeshProUGUI>();
        levelText.text = "Level " + GameControl.currentLevel;
        levelTextObject.SetActive(true);
        currentPRWindow.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelTextObject.SetActive(false);
        currentPRWindow.SetActive(false);
        yield return new WaitForSeconds(1f);

        // Load drones and player

        newPlayer = Instantiate(player);
        Transform trigger = newPlayer.transform.FindChild("Trigger");
        trigger.gameObject.SetActive(false);
        GameControl.dead = false;
        _controlSLA.stopUpdate = false;
        _levelManagerSLA.LoadDrones(GameControl.currentLevel);

        // Countdown
        text3.SetActive(true);
        yield return new WaitForSeconds(1f);
        text3.SetActive(false);
        text2.SetActive(true);
        yield return new WaitForSeconds(1f);
        text2.SetActive(false);
        text1.SetActive(true);
        yield return new WaitForSeconds(1f);
        text1.SetActive(false);

        trigger.gameObject.SetActive(true);
        _scoreSLA.StartScore();

    }
}
