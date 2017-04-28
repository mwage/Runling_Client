using Assets.Scripts.RLR;
using Assets.Scripts.UI;
using Assets.Scripts.UI.RLR_Menu;
using UnityEngine;

public class InGameMenuManagerRLR : MonoBehaviour
{
    public InGameMenuRLR InGameMenu;
    public ControlRLR ControlRLR;
    public OptionsMenu OptionsMenu;
    public HighScoreMenuRLR HighScoreMenuRLR;

    public GameObject InGameMenuObject;
    public GameObject OptionsMenuObject;
    public GameObject HighScoreMenuObject;
    public GameObject WinScreen;
    public GameObject PauseScreen;

    public bool MenuOn;
    private bool _pause;

    private void Awake()
    {
        MenuOn = false;
        OptionsMenu.OptionsMenuActive = false;
        HighScoreMenuRLR.HighScoreMenuActive = false;
        _pause = false;
    }

    public void CloseMenus()
    {
        InGameMenuObject.SetActive(false);
        OptionsMenuObject.SetActive(false);
        HighScoreMenuObject.SetActive(false);
    }

    void Update()
    {
        // Navigate menu with esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MenuOn && !WinScreen.gameObject.activeSelf)
            {
                InGameMenuObject.SetActive(true);
                Time.timeScale = 0;
                MenuOn = true;
            }
            else if (MenuOn == true && OptionsMenu.OptionsMenuActive)
            {
                OptionsMenuObject.SetActive(false);
                OptionsMenu.OptionsMenuActive = false;
                InGameMenuObject.SetActive(true);
            }
            else if (MenuOn == true && HighScoreMenuRLR.HighScoreMenuActive)
            {
                HighScoreMenuObject.SetActive(false);
                HighScoreMenuRLR.HighScoreMenuActive = false;
                InGameMenuObject.SetActive(true);
            }
            else
            {
                InGameMenuObject.SetActive(false);
                Time.timeScale = 1;
                MenuOn = false;
            }
        }

        //pause game
        if (Input.GetKeyDown(KeyCode.Pause))
        {
            if (!_pause)
            {
                Time.timeScale = 0;
                _pause = true;
                PauseScreen.SetActive(true);
            }
            else if (_pause)
            {
                Time.timeScale = 1;
                _pause = false;
                PauseScreen.SetActive(false);
            }
        }
    }
}