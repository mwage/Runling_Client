using System;
using System.Linq;
using Photon;
using UnityEngine;

namespace UI.Main_Menu
{
    public class MultiplayerMenu : PunBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;

        public GameObject OutOfLobby;
        public GameObject CreatingLobby;
        public GameObject InLobby;

        private MainMenu _mainMenu;

        private void Awake()
        {
            _mainMenu = _mainMenuManager.MainMenu;
        }

        private void OpenRoom(string roomName, RoomOptions roomOptions)
        {
            var rooms = PhotonNetwork.GetRoomList();
            var roomNames = rooms.Select(room => room.Name).ToList();
            int i = 0;
            var internalName = roomName + i;

            while (roomNames.Contains(roomName))
            {
                i++;
                internalName = roomName + i;
            }
            PhotonNetwork.CreateRoom(internalName, roomOptions, TypedLobby.Default);
        }

        #region Buttons

        public void CreateRoom()
        {
            OutOfLobby.SetActive(false);
            CreatingLobby.SetActive(true);
        }

        public void CreateRoomRLR()
        {
            var roomOptions = new RoomOptions {IsVisible = true, IsOpen = true, MaxPlayers = 10};

            OpenRoom("Runling Run", roomOptions);
        }

        public void CreateRoomSLA()
        {
            var roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 10 };

            OpenRoom("Arena", roomOptions);
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveRoom();
            InLobby.SetActive(false);
            OutOfLobby.SetActive(true);
        }

        public void BackToMenu()
        {
            gameObject.SetActive(false);
            _mainMenu.gameObject.SetActive(true);
        }
        #endregion

        #region PUN Callbacks

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("Failed to join room");
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Created Room: " + PhotonNetwork.room.Name);
            CreatingLobby.SetActive(false);
            InLobby.SetActive(true);
        }
        #endregion
    }
}