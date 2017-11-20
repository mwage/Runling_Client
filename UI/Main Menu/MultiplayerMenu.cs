using Launcher;
using Network.Rooms;
using Network.Synchronization;
using System.Collections.Generic;
using Network.Synchronization.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    public class MultiplayerMenu : MonoBehaviour
    {
        [SerializeField] private RoomLayoutGroup _roomLayoutGroup;
        [SerializeField] private PlayerLayoutGroup _playerLayoutGroup;
        [SerializeField] private RoomNameInput _roomNameInput;

        public GameObject OutOfLobby;
        public GameObject CreatingLobby;
        public GameObject InLobby;
        public Button StartButton;

        private MainMenuManager _mainMenuManager;
        private MainMenu _mainMenu;

        private void Awake()
        {
            _mainMenuManager = transform.parent.GetComponent<MainMenuManager>();
            _mainMenu = _mainMenuManager.MainMenu;

            RoomManager.onSuccessfulLeaveRoom += OnLeaveRoom;
            RoomManager.onSuccessfulJoinRoom += OnJoinedRoom;
        }

        private void OnDestroy()
        {
            RoomManager.onSuccessfulLeaveRoom -= OnLeaveRoom;
            RoomManager.onSuccessfulJoinRoom -= OnJoinedRoom;
        }

        private void OnEnable()
        {
            if (RoomManager.Instance.CurrentRoom == null)
            {
                OutOfLobby.SetActive(true);
                CreatingLobby.SetActive(false);
                InLobby.SetActive(false);
                Refresh();
            }
        }

        #region Buttons

        private void Update()
        {
            StartButton.interactable = RoomManager.Instance.IsHost;
        }

        public void Refresh()
        {
            _roomLayoutGroup.DeleteOldRooms();
            RoomManager.Instance.GetOpenRooms();
        }

        public void CreateRoom()
        {
            OutOfLobby.SetActive(false);
            CreatingLobby.SetActive(true);
        }

        public void CreateRoomRLR()
        {
            RoomManager.Instance.CreateRoom(_roomNameInput.CustomRoomName, GameType.RunlingRun, true, PlayerColor.Green);
        }

        public void CreateRoomSLA()
        {
            RoomManager.Instance.CreateRoom(_roomNameInput.CustomRoomName, GameType.Arena, true, PlayerColor.Green);
        }

        public void LeaveRoom()
        {
            RoomManager.Instance.LeaveRoom();
        }

        public void Back()
        {
            CreatingLobby.SetActive(false);
            OutOfLobby.SetActive(true);
        }

        public void BackToMenu()
        {
            if (RoomManager.Instance.CurrentRoom == null)
            {
                InLobby.SetActive(false);
                CreatingLobby.SetActive(false);
            }
            gameObject.SetActive(false);
            _mainMenu.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            RoomManager.Instance.StartGame();
        }

        #endregion

        #region Network Callbacks

        public void OnJoinedRoom(List<Player> playerList)
        {
            CreatingLobby.SetActive(false);
            OutOfLobby.SetActive(false);
            InLobby.SetActive(true);
            _playerLayoutGroup.JoinedRoom(playerList);
        }

        public void OnLeaveRoom()
        {
            Refresh();
            OutOfLobby.SetActive(true);
            InLobby.SetActive(false);
            _playerLayoutGroup.RemovePlayers();
        }
        #endregion
    }
}