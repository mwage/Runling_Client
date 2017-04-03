using Assets.Scripts.SLA;
using Assets.Scripts.UI;
using Assets.Scripts.UI.SLA_Menu;
using UnityEngine;

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
    public GameObject pauseScreen;

    public bool menuOn;
    bool pause;

    private void Awake()
    {
        menuOn = false;
        _optionsMenu.optionsMenuActive = false;
        _highScoreMenuSLA.highScoreMenuActive = false;
        pause = false;
    }

    public void CloseMenus()
    {
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(false);
        highScoreMenu.SetActive(false);
    }

    void Update()
    {
        // Navigate menu with esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuOn && !winScreen.gameObject.activeSelf)
            {
                inGameMenu.SetActive(true);
                Time.timeScale = 0;
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
                Time.timeScale = 1;
                menuOn = false;
            }
        }

        //pause game
        if (Input.GetKeyDown(KeyCode.Pause))
        {
            if (!pause)
            {
                Time.timeScale = 0;
                pause = true;
                pauseScreen.SetActive(true);
            }
            else if (pause)
            {
                Time.timeScale = 1;
                pause = false;
                pauseScreen.SetActive(false);
            }
        }
    }
}