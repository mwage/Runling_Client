using System.Linq;
using Photon;
using UI.Main_Menu.MP;
using UnityEngine;

namespace UI.Main_Menu
{
    public class MultiplayerMenu : PunBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;
        [SerializeField] private PlayerLayoutGroup _playerLayoutGroup;

        public GameObject OutOfLobby;
        public GameObject CreatingLobby;
        public GameObject InLobby;
        public SelectGame? ChooseGame; 

        private MainMenu _mainMenu;
        
        private void Awake()
        {
            _mainMenu = _mainMenuManager.MainMenu;
        }

        private static void OpenRoom(string roomName, RoomOptions roomOptions)
        {
            var rooms = PhotonNetwork.GetRoomList();
            var roomNames = rooms.Select(room => room.Name).ToList();
            int i = 0;

            while (roomNames.Contains(roomName + '#' + i))
            {
                i++;
            }
            PhotonNetwork.CreateRoom(roomName + '#' + i, roomOptions, TypedLobby.Default);
        }

        #region Buttons

        public void CreateRoom()
        {
            OutOfLobby.SetActive(false);
            CreatingLobby.SetActive(true);
        }

        public void ShowLobby()
        {
            OutOfLobby.SetActive(false);
            InLobby.SetActive(true);
        }

        public void CreateRoomRLR()
        {
            var roomOptions = new RoomOptions {IsVisible = true, IsOpen = true, MaxPlayers = 10};
            ChooseGame = SelectGame.RunlingRun;
            OpenRoom("Runling Run", roomOptions);
        }

        public void CreateRoomSLA()
        {
            var roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
            ChooseGame = SelectGame.Arena;
            OpenRoom("Arena", roomOptions);
        }

        public void LeaveLobby()
        {
            PhotonNetwork.Disconnect();
        }

        public void BackToMenu()
        {
            if (PhotonNetwork.room == null)
            {
                InLobby.SetActive(false);
                CreatingLobby.SetActive(false);
            }
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

        public override void OnJoinedRoom()
        {
            OutOfLobby.SetActive(false);
            InLobby.SetActive(true);
            _playerLayoutGroup.JoinedRoom();
        }

        public override void OnLeftRoom()
        {
            Debug.Log(PhotonNetwork.lobby);
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log(PhotonNetwork.room);
            Debug.Log(PhotonNetwork.GetRoomList().Length);
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.Log("disc");
            PhotonNetwork.Reconnect();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("connected");
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        #endregion

        public enum SelectGame
        {
            RunlingRun,
            Arena
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("2nd " + PhotonNetwork.GetRoomList().Length);
            OutOfLobby.SetActive(true);
            InLobby.SetActive(false);
        }
    }
}