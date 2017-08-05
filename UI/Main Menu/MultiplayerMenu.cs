using System;
using System.Linq;
using ExitGames.Client.Photon;
using Launcher;
using Photon;
using UI.Main_Menu.MP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    public class MultiplayerMenu : PunBehaviour
    {
        [SerializeField] private MainMenuManager _mainMenuManager;
        [SerializeField] private PlayerLayoutGroup _playerLayoutGroup;
        [SerializeField] private RoomNameInput _roomNameInput;

        public GameObject OutOfLobby;
        public GameObject CreatingLobby;
        public GameObject InLobby;
        public Button StartButton;
        private SelectGame? _chooseGame; 

        private MainMenu _mainMenu;
        
        private void Awake()
        {
            _mainMenu = _mainMenuManager.MainMenu;
        }

        private void OnEnable()
        {
            if (PhotonNetwork.room == null)
            {
                OutOfLobby.SetActive(true);
                CreatingLobby.SetActive(false);
                InLobby.SetActive(false);
            }
        }


        private void OpenRoom(RoomOptions roomOptions)
        {
            var roomName = PickRoomName(_roomNameInput.CustomRoomName);
            var rooms = PhotonNetwork.GetRoomList();
            var roomNames = rooms.Select(room => room.Name).ToList();

            if (roomNames.Contains(roomName))
            {
                PhotonNetwork.JoinRoom(roomName);
            }
            else
            {
                PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            }
        }


        #region Buttons

        private void Update()
        {
            StartButton.interactable = PhotonNetwork.isMasterClient;
        }


        public void CreateRoom()
        {
            OutOfLobby.SetActive(false);
            CreatingLobby.SetActive(true);
        }

        public void CreateRoomRLR()
        {
            var roomOptions = new RoomOptions {IsVisible = true, IsOpen = true, MaxPlayers = 12};
            _chooseGame = SelectGame.RunlingRun;
            OpenRoom(roomOptions);
        }

        public void CreateRoomSLA()
        {
            var roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 8};
            _chooseGame = SelectGame.Arena;
            OpenRoom(roomOptions);
        }

        public void LeaveLobby()
        {
            PhotonNetwork.LeaveRoom();
            OutOfLobby.SetActive(true);
            InLobby.SetActive(false);
        }

        public void StartGame()
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;

            switch ((string) PhotonNetwork.room.CustomProperties["GM"])
            {
                case "RR":
                    Debug.Log("Start RLR Game");
                    PhotonNetwork.LoadLevel(3);
                    break;
                case "AR":
                    Debug.Log("Start Arena Game");
                    PhotonNetwork.LoadLevel(5);
                    break;
                default:
                    Debug.Log("Couldn't load game, invalid selection");
                    PhotonNetwork.LoadLevel(2);
                    break;
            }
        }

        public void Back()
        {
            CreatingLobby.SetActive(false);
            OutOfLobby.SetActive(true);
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

        public override void OnCreatedRoom()
        {
            Debug.Log("Created Room: " + PhotonNetwork.room.Name);
            var t = new Hashtable();
            switch (_chooseGame)
            {
                case SelectGame.RunlingRun:
                    t.Add("GM", "RR");
                    break;
                case SelectGame.Arena:
                    t.Add("GM", "AR");
                    break;
                default:
                    Debug.Log("No Gamemode assigned");
                    break;
            }

            PhotonNetwork.room.SetCustomProperties(t);
            PhotonNetwork.room.SetPropertiesListedInLobby(new[] {"GM"});

            CreatingLobby.SetActive(false);
            InLobby.SetActive(true);
        }

        public override void OnJoinedRoom()
        {
            GameControl.GameState.Solo = false;
            GameControl.GameState.CurrentLevel = 1;
            OutOfLobby.SetActive(false);
            InLobby.SetActive(true);
            _playerLayoutGroup.JoinedRoom();
        }

        #endregion

        private static string PickRoomName(string roomName)
        {
            if (roomName == "")
            {
                return PhotonNetwork.playerName + "'s Lobby";
            }

            return roomName;
        }

        private enum SelectGame
        {
            RunlingRun,
            Arena
        }
    }
}