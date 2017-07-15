using UnityEngine;

namespace UI.Main_Menu
{
    public class SoloMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        private MainMenu _mainMenu;
        private SLAMenu _slaMenu;
        private RLRMenu _rlrMenu;
        
        private void Awake()
        {
            _mainMenu = _mainMenuManager.MainMenu;
            _slaMenu = _mainMenuManager.SLAMenu;
            _rlrMenu = _mainMenuManager.RLRMenu;
        }

        #region Buttons

        public void SLA()
        {
            gameObject.SetActive(false);
            _slaMenu.gameObject.SetActive(true);
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosSLA, _mainMenuManager.CameraRotSLA);
        }

        public void RLR()
        {
            gameObject.SetActive(false);
            _rlrMenu.gameObject.SetActive(true);
            _mainMenuManager.MoveCamera(_mainMenuManager.CameraPosRLR, _mainMenuManager.CameraRotRLR);
        }

        public void BackToMenu()
        {
            gameObject.SetActive(false);
            _mainMenu.gameObject.SetActive(true);
        }
        #endregion
    }
}