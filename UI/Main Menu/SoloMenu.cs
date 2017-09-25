using Photon;
using UnityEngine;

namespace UI.Main_Menu
{
    public class SoloMenu : PunBehaviour
    {
        public GameObject NormalSolo;
        public GameObject Error;

        private MainMenuManager _mainMenuManager;
        private MainMenu _mainMenu;
        private SLAMenu _slaMenu;
        private RLRMenu _rlrMenu;
        
        private void Awake()
        {
            _mainMenuManager = transform.parent.GetComponent<MainMenuManager>();
            _mainMenu = _mainMenuManager.MainMenu;
            _slaMenu = _mainMenuManager.SLAMenu;
            _rlrMenu = _mainMenuManager.RLRMenu;
        }

        private void OnEnable()
        {
            if (PhotonNetwork.room == null)
            {
                Error.SetActive(false);
                NormalSolo.SetActive(true);
            }
            else
            {
                NormalSolo.SetActive(false);
                Error.SetActive(true);
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
            Error.SetActive(false);
            NormalSolo.SetActive(true);
        }
        #endregion
    }
}