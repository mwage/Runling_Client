using System.Collections.Generic;
using Network.Synchronization.Data;
using UnityEngine;

namespace Network.Rooms
{
    public class RoomLayoutGroup : MonoBehaviour
    {
        public GameObject RoomListingPrefab;
        private List<RoomListing> _roomListings = new List<RoomListing>();

        private void Awake()
        {
            RoomManager.onReceivedOpenRooms += OnReceivedOpenRooms;
        }

        private void OnDestroy()
        {
            RoomManager.onReceivedOpenRooms -= OnReceivedOpenRooms;
        }

        public void OnReceivedOpenRooms(List<Room> roomList)
        {
            foreach (var room in roomList)
            {
                var roomListing = Instantiate(RoomListingPrefab, transform, false).GetComponent<RoomListing>();
                roomListing.Initialize(room);
                _roomListings.Add(roomListing);
            }
        }

        public void DeleteOldRooms()
        {
            foreach (var roomListing in _roomListings)
            {
                Destroy(roomListing.gameObject);
            }
            _roomListings = new List<RoomListing>();
        }
    }
}

