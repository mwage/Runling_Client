using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManagerSLA : MonoBehaviour
{
    public InGameMenuSLA _inGameMenu;
    public ControlSLA _controlSLA;
    public OptionsMenu _optionsMenu;
    public HighScoreMenuSLA _highScoreMenuSLA;

    public GameObject inGameMenu;
    public GameObject optionsMenu;
    public GameObject highScoreMenu;
    public GameObject winScreen;

    public bool menuOn;

    private void Awake()
    {
        menuOn = false;
        _optionsMenu.optionsMenuActive = false;
        _highScoreMenuSLA.highScoreMenuActive = false;
    }

    public void CloseMenus()
    {
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(false);
        highScoreMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOn == false && !winScreen.gameObject.activeSelf)
            {
                inGameMenu.SetActive(true);
                menuOn = true;
            }
            else if (menuOn == true && _optionsMenu.optionsMenuActive)
            {
                optionsMenu.SetActive(false);
                _optionsMenu.optionsMenuActive = false;
                inGameMenu.SetActive(true);
            }
            else if (menuOn == true && _highScoreMenuSLA.highScoreMenuActive)
            {
                highScoreMenu.SetActive(false);
                _highScoreMenuSLA.highScoreMenuActive = false;
                inGameMenu.SetActive(true);
            }
            else
            {
                inGameMenu.SetActive(false);
                menuOn = false;
            }
        }
    }
}