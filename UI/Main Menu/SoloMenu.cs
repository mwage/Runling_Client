using Photon;
using UnityEngine;

namespace UI.Main_Menu
{
    public class SoloMenu : PunBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;
        [SerializeField] private GameObject _normalSolo;
        [SerializeField] private GameObject _error;

        private MainMenu _mainMenu;
        private SLAMenu _slaMenu;
        private RLRMenu _rlrMenu;
        
        private void Awake()
        {
            _mainMenu = _mainMenuManager.MainMenu;
            _slaMenu = _mainMenuManager.SLAMenu;
            _rlrMenu = _mainMenuManager.RLRMenu;
        }

        private void OnEnable()
        {
            if (PhotonNetwork.room == null)
            {
                _error.SetActive(false);
                _normalSolo.SetActive(true);
            }
            else
            {
                _normalSolo.SetActive(false);
                _error.SetActive(true);
            }
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

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region PUN Callbacks

        public override void OnJoinedLobby()
        {
            _error.SetActive(false);
            _normalSolo.SetActive(true);
        }
        #endregion
    }
}