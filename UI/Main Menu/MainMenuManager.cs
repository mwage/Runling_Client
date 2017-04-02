using Assets.Scripts.UI.SLA_Menu;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class MainMenuManager : MonoBehaviour {

        public MainMenu _mainMenu;
        public OptionsMenu _optionsMenu;
        public SLAMenu _sLAMenu;
        public HighScoreMenuSLA _highScoreMenuSLA;

        public GameObject mainMenu;
        public GameObject optionsMenu;
        public GameObject sLAMenu;
        public GameObject highScoreMenu;
 
        private void Awake()
        {
            _optionsMenu.optionsMenuActive = false;
            _sLAMenu.SLAMenuActive = false;
            _highScoreMenuSLA.highScoreMenuActive = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (_optionsMenu.optionsMenuActive)
                {
                    optionsMenu.SetActive(false);
                    _optionsMenu.optionsMenuActive = false;
                    mainMenu.SetActive(true);
                }
                else if (_sLAMenu.SLAMenuActive)
                {
                    sLAMenu.SetActive(false);
                    _sLAMenu.SLAMenuActive = false;
                    mainMenu.SetActive(true);
                }
                else if (_highScoreMenuSLA.highScoreMenuActive)
                {
                    highScoreMenu.SetActive(false);
                    _highScoreMenuSLA.highScoreMenuActive = false;
                    sLAMenu.SetActive(true);
                    _sLAMenu.SLAMenuActive = true;
                }
            }
        }
    }
}