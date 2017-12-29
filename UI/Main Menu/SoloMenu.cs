using Network.Rooms;
using UnityEngine;

namespace UI.Main_Menu
{
    public class SoloMenu : MonoBehaviour
    {
        public GameObject NormalSolo;
        public GameObject Error;

        private MainMenuManager _mainMenuManager;

        private SLAMenu _slaMenu;
        private RLRMenu _rlrMenu;
        
        private void Awake()
        {
            _mainMenuManager = transform.parent.GetComponent<MainMenuManager>();
            _slaMenu = _mainMenuManager.SLAMenu;
            _rlrMenu = _mainMenuManager.RLRMenu;

            RoomManager.onSuccessfulLeaveRoom += OnLeaveRoom;
        }

        private void OnDestroy()
        {
            RoomManager.onSuccessfulLeaveRoom -= OnLeaveRoom;
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
        }

        #endregion

        private void OnLeaveRoom()
        {
            Error.SetActive(false);
            NormalSolo.SetActive(true);
        }
    }
}