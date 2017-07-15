using System.Collections.Generic;
using Launcher;
using Photon;
using UnityEngine;

namespace UI.Main_Menu.MP
{
    public class RoomLayoutGroup : PunBehaviour
    {
        [SerializeField] private MultiplayerMenu _multiplayerMenu;
        [SerializeField] private GameObject _roomListingPrefab;
        private List<RoomListing> _roomList = new List<RoomListing>();

        public override void OnReceivedRoomListUpdate()
        {
            Debug.Log("recieved room update");

            var rooms = PhotonNetwork.GetRoomList();
            Debug.Log(rooms.Length + " - " + PhotonNetwork.GetRoomList().Length);
            foreach (var room in rooms)
            {
                Debug.Log(room.Name);
                RoomRecieved(room);
            }
            
            RemoveOldRooms();
        }

        private void RoomRecieved(RoomInfo room)
        {
            var index = _roomList.FindIndex(x => x.RoomName == room.Name);
            if (index == -1)
            {
                if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
                {
                    var roomListingObject = Instantiate(_roomListingPrefab, transform, false);
                    Debug.Log("instantiating prefab - " + roomListingObject);

                    var roomListing = roomListingObject.GetComponent<RoomListing>();
                    _roomList.Add(roomListing);

                    index = _roomList.Count - 1;
                }
            }
            
            if (index != -1)
            {
                var roomListing = _roomList[index];
                roomListing.SetRoomNameText(room.Name);
                roomListing.Updated = true;
            }
        }

        private void RemoveOldRooms()
        {
            var removeRooms = new List<RoomListing>();
            foreach (var roomListing in _roomList)
            {
                if (!roomListing.Updated)
                {
                    removeRooms.Add(roomListing);
                }
                else
                {
                    roomListing.Updated = false;
                }
            }

            foreach (var roomListing in removeRooms)
            {
                _roomList.Remove(roomListing);
                Debug.Log("Destroying: " + roomListing.RoomName);
                Destroy(roomListing.gameObject);

            }
        }
    }
}

