using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using Launcher;
using Network.Chat;
using Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.Rooms
{
    public class RoomManager : Singleton<RoomManager>
    {
        protected RoomManager()
        {
        }

        public bool IsHost { get; private set; }
        public Room CurrentRoom { get; set; }

        #region Events

        public delegate void SuccessfulLeaveRoomEventHandler();
        public delegate void SuccessfulJoinRoomEventHandler(List<Player> playerList);
        public delegate void ReceivedOpenRoomsEventHandler(List<Room> roomList);
        public delegate void PlayerJoinedEventHandler(Player player);
        public delegate void PlayerLeftEventHandler(uint leftId, uint newHostId);
        public delegate void StartGameEventHandler();

        public static event SuccessfulLeaveRoomEventHandler onSuccessfulLeaveRoom;
        public static event SuccessfulJoinRoomEventHandler onSuccessfulJoinRoom;
        public static event ReceivedOpenRoomsEventHandler onReceivedOpenRooms;
        public static event PlayerJoinedEventHandler onPlayerJoined;
        public static event PlayerLeftEventHandler onPlayerLeft;
        public static event StartGameEventHandler onStartGame;

        #endregion
        
        private void Awake()
        {
            MainClient.Instance.MessageReceived += OnDataHandler;
        }

        public override void OnDestroy()
        {
            if (MainClient.Instance != null)
            {
                MainClient.Instance.MessageReceived -= OnDataHandler;
            }
            base.OnDestroy();
        }

        #region Network Calls

        public void CreateRoom(string roomname, GameType gameType, bool isVisible, PlayerColor color)
        {
            var writer = new DarkRiftWriter();
            writer.Write(roomname);
            writer.Write((byte) gameType);
            writer.Write(isVisible);
            writer.Write((byte) color);

            MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.Room, RoomSubjects.Create, writer), SendMode.Reliable);
        }

        public void JoinRoom(ushort roomId, PlayerColor color)
        {
            var writer = new DarkRiftWriter();
            writer.Write(roomId);
            writer.Write((byte)color);

            MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.Room, RoomSubjects.Join, writer), SendMode.Reliable);
        }

        public void LeaveRoom()
        {
            MainClient.Instance.SendMessage(
                new TagSubjectMessage(Tags.Room, RoomSubjects.Leave, new DarkRiftWriter()), SendMode.Reliable);
        }

        public void GetOpenRooms()
        {
            MainClient.Instance.SendMessage(
                new TagSubjectMessage(Tags.Room, RoomSubjects.GetOpenRooms, new DarkRiftWriter()), SendMode.Reliable);
        }

        public void StartGame()
        {
            var writer = new DarkRiftWriter();
            writer.Write(Instance.CurrentRoom.Id);
            MainClient.Instance.SendMessage(new TagSubjectMessage(Tags.Room, RoomSubjects.StartGame, writer), SendMode.Reliable);
        }

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.Room)
                return;

            // Successfully created Room
            if (message.Subject == RoomSubjects.CreateSuccess)
            {
                var reader = message.GetReader();
                var room = reader.ReadSerializable<Room>();
                var player = reader.ReadSerializable<Player>();

                IsHost = player.IsHost;
                CurrentRoom = room;
                ChatManager.Instance.ServerMessage("Created Lobby " + room.Name + "!", MessageType.Room);

                onSuccessfulJoinRoom?.Invoke(new List<Player> {player});
            }

            // Failed to create Room
            else if (message.Subject == RoomSubjects.CreateFailed)
            {
                ChatManager.Instance.ServerMessage("Failed to create Lobby.", MessageType.Error);
                var reader = message.GetReader();
                if (reader.Length != 1)
                {
                    Debug.LogWarning("Invalid CreateRoomFailed Error data received.");
                    return;
                }

                switch (reader.ReadByte())
                {
                    case 0:
                        Debug.Log("Invalid CreateRoom data sent!");
                        break;
                    case 1:
                        Debug.Log("Player not logger in!");
                        SceneManager.LoadScene("Login");
                        break;
                    default:
                        Debug.Log("Invalid errorId!");
                        break;
                }
            }

            // Successfully joined Room
            else if (message.Subject == RoomSubjects.JoinSuccess)
            {
                var reader = message.GetReader();
                var playerList = new List<Player>();

                var room = reader.ReadSerializable<Room>();
                while (reader.Position < reader.Length)
                {
                    var player = reader.ReadSerializable<Player>();
                    playerList.Add(player);
                    ChatManager.Instance.ServerMessage(player.Name + "has joined the Lobby.", MessageType.Room);
                }

                IsHost = playerList.Find(p => p.Id == MainClient.Instance.Id).IsHost;
                CurrentRoom = room;
                onSuccessfulJoinRoom?.Invoke(playerList);
            }

            // Failed to join Room
            else if (message.Subject == RoomSubjects.JoinFailed)
            {
                var content = "Failed to join Lobby.";
                var reader = message.GetReader();
                if (reader.Length != 1)
                {
                    Debug.LogWarning("Invalid JoinRoomRoomFailed Error data received.");
                }
                else
                {
                    switch (reader.ReadByte())
                    {
                        case 0:
                            Debug.Log("Invalid JoinRoom data sent!");
                            break;
                        case 1:
                            Debug.Log("Player not logger in!");
                            SceneManager.LoadScene("Login");
                            break;
                        case 2:
                            Debug.Log("Player already is in a room!");
                            content = "Already in a Lobby.";
                            break;
                        case 3:
                            Debug.Log("Room doesn't exist anymore or has already started.");
                            content = "The room doesn't exist anymore or has already started.";
                            break;
                        default:
                            Debug.Log("Invalid errorId!");
                            break;
                    }
                }
                ChatManager.Instance.ServerMessage(content, MessageType.Error);
            }

            // Successfully left Room
            else if (message.Subject == RoomSubjects.LeaveSuccess)
            {
                ChatManager.Instance.ServerMessage("You have left the Lobby.", MessageType.Room);
                CurrentRoom = null;
                onSuccessfulLeaveRoom?.Invoke();
            }

            // Another player joined the Room
            else if (message.Subject == RoomSubjects.PlayerJoined)
            {
                var reader = message.GetReader();
                var player = reader.ReadSerializable<Player>();
                ChatManager.Instance.ServerMessage(player.Name + " has joined the Lobby.", MessageType.Room);

                onPlayerJoined?.Invoke(player);
            }

            // Another player left the Room
            else if (message.Subject == RoomSubjects.PlayerLeft)
            {
                var reader = message.GetReader();
                var leftId = reader.ReadUInt32();
                var newHostId = reader.ReadUInt32();
                var leaverName = reader.ReadString();
                ChatManager.Instance.ServerMessage(leaverName + " has left the Lobby.", MessageType.Room);

                if (newHostId == MainClient.Instance.Id)
                {
                    IsHost = true;
                }

                onPlayerLeft?.Invoke(leftId, newHostId);
            }

            // Received all available Rooms
            else if (message.Subject == RoomSubjects.GetOpenRooms)
            {
                var reader = message.GetReader();
                var roomList = new List<Room>();

                while (reader.Position < reader.Length)
                {
                    roomList.Add(reader.ReadSerializable<Room>());
                }
                onReceivedOpenRooms?.Invoke(roomList);
            }

            // Failed to receive all available Rooms
            else if (message.Subject == RoomSubjects.GetOpenRoomsFailed)
            {
                Debug.Log("Player not logged in!");
                SceneManager.LoadScene("Login");
            }

//             TODO: color

            // Successfully initialized Start
            else if (message.Subject == RoomSubjects.StartGameSuccess)
            {
                var reader = message.GetReader();
                MainClient.Instance.GameServerPort = reader.ReadUInt16();
                GameControl.GameState.Solo = false;
            }

            // Failed to start Game
            else if (message.Subject == RoomSubjects.StartGameFailed)
            {
                var content = "Failed to start game.";
                var reader = message.GetReader();
                if (reader.Length != 1)
                {
                    Debug.LogWarning("Invalid StartGame Error data received.");
                    return;
                }

                switch (reader.ReadByte())
                {
                    case 0:
                        Debug.Log("Invalid CreateRoom data sent!");
                        break;
                    case 1:
                        Debug.Log("Player not logged in!");
                        SceneManager.LoadScene("Login");
                        break;
                    case 2:
                        content = "Only the host can start a game!";
                        break;
                    case 3:
                        content = "Currently no gameserver is available!";
                        break;
                    default:
                        Debug.Log("Invalid errorId!");
                        break;
                }
                ChatManager.Instance.ServerMessage(content, MessageType.Error);
            }

            // Server ready, launch the game
            if (message.Subject == RoomSubjects.ServerReady)
            {
                if (CurrentRoom == null)
                {
                    Debug.Log("CurrentRoom not set.");
                    return;
                }


                switch (CurrentRoom.GameType)
                {
                    case GameType.Arena:
                        SceneManager.LoadScene("SLA");
                        break;
                    case GameType.RunlingRun:
                        SceneManager.LoadScene("RLR");
                        break;
                    default:
                        Debug.Log("Invalid GameType");
                        LeaveRoom();
                        break;
                }
            }
        }
    }
}
