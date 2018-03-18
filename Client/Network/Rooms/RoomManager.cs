using Client.Scripts.Launcher;
using Client.Scripts.Network.Chat;
using DarkRift;
using DarkRift.Client;
using Game.Scripts;
using Game.Scripts.Network.DarkRiftTags;
using Game.Scripts.Network.Data;
using System.Collections.Generic;
using Game.Scripts.GameSettings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.Network.Rooms
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
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(roomname);
                writer.Write((byte) gameType);
                writer.Write(isVisible);
                writer.Write((byte) color);

                using (var msg = Message.Create(RoomTags.Create, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public void JoinRoom(ushort roomId, PlayerColor color)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(roomId);
                writer.Write((byte)color);

                using (var msg = Message.Create(RoomTags.Join, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public void LeaveRoom()
        {
            using (var msg = Message.CreateEmpty(RoomTags.Leave))
            {
                MainClient.Instance.SendMessage(msg, SendMode.Reliable);
            }
        }

        public void GetOpenRooms()
        {
            using (var msg = Message.CreateEmpty(RoomTags.GetOpenRooms))
            {
                MainClient.Instance.SendMessage(msg, SendMode.Reliable);
            }
        }

        public void StartGame()
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(Instance.CurrentRoom.Id);

                using (var msg = Message.Create(RoomTags.StartGame, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        #endregion

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.Room || message.Tag >= Tags.TagsPerPlugin * (Tags.Room + 1))
                    return;

                // Successfully created Room
                switch (message.Tag)
                {
                    case RoomTags.CreateSuccess:
                    {
                        using (var reader = message.GetReader())
                        {
                            var room = reader.ReadSerializable<Room>();
                            var player = reader.ReadSerializable<Player>();

                            IsHost = player.IsHost;
                            CurrentRoom = room;
                            ChatManager.Instance.ServerMessage("Created Lobby " + room.Name + "!", MessageType.Room);

                            onSuccessfulJoinRoom?.Invoke(new List<Player> {player});
                        }
                        break;
                    }
                    case RoomTags.CreateFailed:
                    {
                        ChatManager.Instance.ServerMessage("Failed to create Lobby.", MessageType.Error);
                        using (var reader = message.GetReader())
                        {
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
                        break;
                    }
                    case RoomTags.JoinSuccess:
                    {
                        var playerList = new List<Player>();
                        Room room;
                        using (var reader = message.GetReader())
                        {
                            room = reader.ReadSerializable<Room>();
                            while (reader.Position < reader.Length)
                            {
                                var player = reader.ReadSerializable<Player>();
                                playerList.Add(player);
                                ChatManager.Instance.ServerMessage(player.Name + "has joined the Lobby.", MessageType.Room);
                            }
                        }

                        IsHost = playerList.Find(p => p.Id == MainClient.Instance.Id).IsHost;
                        CurrentRoom = room;
                        onSuccessfulJoinRoom?.Invoke(playerList);
                        break;
                    }
                    case RoomTags.JoinFailed:
                    {
                        var content = "Failed to join Lobby.";
                        using (var reader = message.GetReader())
                        {
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
                        }
                        ChatManager.Instance.ServerMessage(content, MessageType.Error);
                        break;
                    }
                    case RoomTags.LeaveSuccess:
                    {
                        ChatManager.Instance.ServerMessage("You have left the Lobby.", MessageType.Room);
                        CurrentRoom = null;
                        onSuccessfulLeaveRoom?.Invoke();
                        break;
                    }

                    case RoomTags.PlayerJoined:
                    {
                        using (var reader = message.GetReader())
                        {
                            var player = reader.ReadSerializable<Player>();
                            ChatManager.Instance.ServerMessage(player.Name + " has joined the Lobby.", MessageType.Room);

                            onPlayerJoined?.Invoke(player);
                        }
                        break;
                    }
                    case RoomTags.PlayerLeft:
                    {
                        using (var reader = message.GetReader())
                        {
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
                        break;
                    }
                    case RoomTags.GetOpenRooms:
                    {
                        var roomList = new List<Room>();
                        using (var reader = message.GetReader())
                        {
                            while (reader.Position < reader.Length)
                            {
                                roomList.Add(reader.ReadSerializable<Room>());
                            }
                        }
                        onReceivedOpenRooms?.Invoke(roomList);
                        break;
                    }
                    case RoomTags.GetOpenRoomsFailed:
                    {
                        Debug.Log("Player not logged in!");
                        SceneManager.LoadScene("Login");
                        break;
                    }
                    case RoomTags.StartGameSuccess:
                    {
                        using (var reader = message.GetReader())
                        {
                            MainClient.Instance.GameServerPort = reader.ReadUInt16();
                            GameControl.GameState.Solo = false;
                            onStartGame?.Invoke();
                        }
                        break;
                    }
                    case RoomTags.StartGameFailed:
                    {
                        var content = "Failed to start game.";
                        using (var reader = message.GetReader())
                        {
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
                        }
                        ChatManager.Instance.ServerMessage(content, MessageType.Error);
                        break;
                    }
                    case RoomTags.ServerReady:
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
                        break;
                    }
                        
                }
            }
        }
    }
}
