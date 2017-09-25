using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using Launcher;
using Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.Rooms
{
    public class RoomManager : MonoBehaviour
    {
        public static bool IsHost { get; private set; }
        public static Room CurrentRoom { get; set; }

        public delegate void SuccessfulCreatedRoomEventHandler(Player player);
        public delegate void SuccessfulLeaveRoomEventHandler();
        public delegate void SuccessfulJoinRoomEventHandler(List<Player> playerList);
        public delegate void ReceivedOpenRoomsEventHandler(List<Room> roomList);
        public delegate void PlayerJoinedEventHandler(Player player);
        public delegate void PlayerLeftEventHandler(uint leftId, uint newHostId);

        public static event SuccessfulCreatedRoomEventHandler onSuccessfulCreatedRoom;
        public static event SuccessfulLeaveRoomEventHandler onSuccessfulLeaveRoom;
        public static event SuccessfulJoinRoomEventHandler onSuccessfulJoinRoom;
        public static event ReceivedOpenRoomsEventHandler onReceivedOpenRooms;
        public static event PlayerJoinedEventHandler onPlayerJoined;
        public static event PlayerLeftEventHandler onPlayerLeft;

        private void Awake()
        {
            GameControl.Client.MessageReceived += OnDataHandler;
        }

        private void OnDestroy()
        {
            GameControl.Client.MessageReceived -= OnDataHandler;
        }

        #region Network Calls

        public static void CreateRoom(string roomname, GameType gameType, bool isVisible, PlayerColor color)
        {
            var writer = new DarkRiftWriter();
            writer.Write(roomname);
            writer.Write((byte) gameType);
            writer.Write(isVisible);
            writer.Write((byte) color);

            GameControl.Client.SendMessage(new TagSubjectMessage(Tags.Room, RoomSubjects.Create, writer),
                SendMode.Reliable);
        }

        public static void JoinRoom(ushort roomId, PlayerColor color)
        {
            var writer = new DarkRiftWriter();
            writer.Write(roomId);
            writer.Write((byte)color);

            GameControl.Client.SendMessage(new TagSubjectMessage(Tags.Room, RoomSubjects.Join, writer),
                SendMode.Reliable);
        }

        public static void LeaveRoom()
        {
            Debug.Log("leaving");
            GameControl.Client.SendMessage(
                new TagSubjectMessage(Tags.Room, RoomSubjects.Leave, new DarkRiftWriter()),
                SendMode.Reliable);
        }

        public static void GetOpenRooms()
        {
            GameControl.Client.SendMessage(
                new TagSubjectMessage(Tags.Room, RoomSubjects.GetOpenRooms, new DarkRiftWriter()),
                SendMode.Reliable);
        }
        #endregion

        private static void OnDataHandler(object sender, MessageReceivedEventArgs e)
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
                onSuccessfulCreatedRoom?.Invoke(player);
            }

            // Failed to create Room
            else if (message.Subject == RoomSubjects.CreateFailed)
            {
                var reader = message.GetReader();
                if (reader.Length != 1)
                {
                    Debug.LogWarning("Invalid CreateRoomFailed Error data received.");
                    return;
                }
                // If player is not logged in
                if (reader.ReadByte() == 2)
                {
                    Debug.Log("Player not logger in!");
                    SceneManager.LoadScene("Login");
                }
                // Invalid data sent to join room
                else
                {
                    Debug.Log("Invalid CreateRoom data sent!");
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
                    playerList.Add(reader.ReadSerializable<Player>());
                }

                IsHost = playerList.Find(p => p.Id == GameControl.Client.ID).IsHost;
                CurrentRoom = room;
                onSuccessfulJoinRoom?.Invoke(playerList);
            }

            // Failed to join Room
            else if (message.Subject == RoomSubjects.JoinFailed)
            {
                var reader = message.GetReader();
                if (reader.Length != 1)
                {
                    Debug.LogWarning("Invalid JoinRoomRoomFailed Error data received.");
                    return;
                }
                var errorId = reader.ReadByte();

                // If player is already in a room
                if (errorId == 1)
                {
                    Debug.Log("Player already is in a room!");
                }
                // If player is not logged in
                else if (errorId == 2)
                {
                    Debug.Log("Player not logger in!");
                    SceneManager.LoadScene("Login");
                }
                // If room doesn't exist anymore
                else if (errorId == 3)
                {
                    Debug.Log("Room doesn't exist anymore");
                }
                // Invalid data sent to join room
                else
                {
                    Debug.Log("Invalid JoinRoom data sent!");
                }
            }

            // Successfully left Room
            else if (message.Subject == RoomSubjects.LeaveSuccess)
            {
                CurrentRoom = null;
                onSuccessfulLeaveRoom?.Invoke();
            }

            // Another player joined the Room
            else if (message.Subject == RoomSubjects.PlayerJoined)
            {
                var reader = message.GetReader();
                onPlayerJoined?.Invoke(reader.ReadSerializable<Player>());
            }

            // Another player left the Room
            else if (message.Subject == RoomSubjects.PlayerLeft)
            {
                var reader = message.GetReader();
                var leftId = reader.ReadUInt32();
                var newHostId = reader.ReadUInt32();

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
                Debug.Log("Player not logger in!");
                SceneManager.LoadScene("Login");
            }

            // TODO: color
        }
    }
}
