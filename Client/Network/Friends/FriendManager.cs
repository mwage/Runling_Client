using Client.Scripts.Network.Chat;
using DarkRift;
using DarkRift.Client;
using Game.Scripts;
using Game.Scripts.Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.Network.Friends
{
    public class FriendManager : Singleton<FriendManager>
    {
        protected FriendManager()
        {
        }

        #region Events

        public delegate void SuccessfulFriendRequestEventHandler(string friendName);
        public delegate void NewFriendRequestEventHandler(string friendName);
        public delegate void SuccessfulDeclineRequestEventHandler(string friendName);
        public delegate void SuccessfulAcceptRequestEventHandler(string friendName, bool online);
        public delegate void SuccessfulRemoveFriendEventHandler(string friendName);
        public delegate void SuccessfulGetAllFriendsEventHandler(string[] onlineFriends, string[] offlineFriends, 
            string[] openRequests, string[] unansweredRequests);
        public delegate void FriendLoginEventHandler(string friendName);
        public delegate void FriendLogoutEventHandler(string friendName);

        public static event SuccessfulFriendRequestEventHandler onSuccessfulFriendRequest;
        public static event NewFriendRequestEventHandler onNewFriendRequest;
        public static event SuccessfulDeclineRequestEventHandler onSuccessfulDeclineRequest;
        public static event SuccessfulAcceptRequestEventHandler onSuccessfulAcceptRequest;
        public static event SuccessfulRemoveFriendEventHandler onSuccessfulRemoveFriend;
        public static event SuccessfulGetAllFriendsEventHandler onSuccessfulGetAllFriends;
        public static event FriendLoginEventHandler onFriendLogin;
        public static event FriendLogoutEventHandler onFriendLogout;

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

        public void SendFriendRequest(string friendName)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(friendName);

                using (var msg = Message.Create(FriendTags.FriendRequest, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public void DeclineFriendRequest(string friendName)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(friendName);

                using (var msg = Message.Create(FriendTags.DeclineRequest, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public void AcceptFriendRequest(string friendName)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(friendName);

                using (var msg = Message.Create(FriendTags.AcceptRequest, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public void RemoveFriend(string friendName)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(friendName);

                using (var msg = Message.Create(FriendTags.RemoveFriend, writer))
                {
                    MainClient.Instance.SendMessage(msg, SendMode.Reliable);
                }
            }
        }

        public void GetAllFriends()
        {
            using (var msg = Message.CreateEmpty(FriendTags.GetAllFriends))
            {
                MainClient.Instance.SendMessage(msg, SendMode.Reliable);
            }
        }

        #endregion

        private static void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.Friends || message.Tag >= Tags.TagsPerPlugin * (Tags.Friends + 1))
                    return;
                
                switch (message.Tag)
                {
                    // New request
                    case FriendTags.FriendRequest:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            ChatManager.Instance.ServerMessage(friendName + " wants to add you as a friend!", MessageType.Info);

                            onNewFriendRequest?.Invoke(friendName);
                        }
                        break;
                    }
                    // Request sent
                    case FriendTags.RequestSuccess:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            ChatManager.Instance.ServerMessage("Friend request sent.", MessageType.Info);

                            onSuccessfulFriendRequest?.Invoke(friendName);
                        }
                        break;
                    }
                    case FriendTags.RequestFailed:
                    {
                        var content = "Failed to send friend request.";
                        using (var reader = message.GetReader())
                        {
                            if (reader.Length != 1)
                            {
                                Debug.LogWarning("Invalid RequestFailed Error data received.");
                            }
                            else
                            {
                                switch (reader.ReadByte())
                                {
                                    case 0:
                                        Debug.Log("Invalid Friend Request data sent!");
                                        break;
                                    case 1:
                                        Debug.Log("Player not logged in!");
                                        SceneManager.LoadScene("Login");
                                        break;
                                    case 2:
                                        Debug.Log("Database Error");
                                        break;
                                    case 3:
                                        Debug.Log("No user with that name found!");
                                        content = "Username doesn't exist.";
                                        break;
                                    case 4:
                                        Debug.Log("Friend request failed. You are already friends or have an open request");
                                        content = "You are already friends or have an open request with this player.";
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
                    // Request declined
                    case FriendTags.DeclineRequestSuccess:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            var isSender = reader.ReadBoolean();
                            var content = isSender
                                ? "Declined " + friendName + "'s friend request."
                                : friendName + " declined your friend request.";
                            ChatManager.Instance.ServerMessage(content, MessageType.Error);

                            onSuccessfulDeclineRequest?.Invoke(friendName);
                        }
                        break;
                    }
                    case FriendTags.DeclineRequestFailed:
                    {
                        ChatManager.Instance.ServerMessage("Failed to decline request.", MessageType.Error);

                        using (var reader = message.GetReader())
                        {
                            if (reader.Length != 1)
                            {
                                Debug.LogWarning("Invalid DeclineRequestFailed Error data received.");
                                return;
                            }

                            switch (reader.ReadByte())
                            {
                                case 0:
                                    Debug.Log("Invalid Decline Request data sent!");
                                    break;
                                case 1:
                                    Debug.Log("Player not logged in!");
                                    SceneManager.LoadScene("Login");
                                    break;
                                case 2:
                                    Debug.Log("Database Error");
                                    break;
                                default:
                                    Debug.Log("Invalid errorId!");
                                    break;
                            }
                        }
                        break;
                    }
                    case FriendTags.AcceptRequestSuccess:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            var isSender = reader.ReadBoolean();
                            ChatManager.Instance.ServerMessage("Added " + friendName + " to your friendlist.",
                                MessageType.Info);

                            onSuccessfulAcceptRequest?.Invoke(friendName, isSender);
                        }
                        break;
                    }
                    case FriendTags.AcceptRequestFailed:
                    {
                        ChatManager.Instance.ServerMessage("Failed to accept request.", MessageType.Error);

                        using (var reader = message.GetReader())
                        {
                            if (reader.Length != 1)
                            {
                                Debug.LogWarning("Invalid DeclineRequestFailed Error data received.");
                                return;
                            }

                            switch (reader.ReadByte())
                            {
                                case 0:
                                    Debug.Log("Invalid Accept Request data sent!");
                                    break;
                                case 1:
                                    Debug.Log("Player not logged in!");
                                    SceneManager.LoadScene("Login");
                                    break;
                                case 2:
                                    Debug.Log("Database Error");
                                    break;
                                default:
                                    Debug.Log("Invalid errorId!");
                                    break;
                            }
                        }
                        break;
                    }
                    case FriendTags.RemoveFriendSuccess:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            var isSender = reader.ReadBoolean();
                            var content = isSender
                                ? "Removed " + friendName + " from your friendlist."
                                : friendName + " removed you from his friendlist.";
                            ChatManager.Instance.ServerMessage(content, MessageType.Error);

                            onSuccessfulRemoveFriend?.Invoke(friendName);
                        }
                        break;
                    }
                    case FriendTags.RemoveFriendFailed:
                    {
                        ChatManager.Instance.ServerMessage("Failed to remove friend.", MessageType.Error);

                        using (var reader = message.GetReader())
                        {
                            if (reader.Length != 1)
                            {
                                Debug.LogWarning("Invalid RemoveFriend Error data received.");
                                return;
                            }

                            switch (reader.ReadByte())
                            {
                                case 0:
                                    Debug.Log("Invalid Remove Friend data sent!");
                                    break;
                                case 1:
                                    Debug.Log("Player not logged in!");
                                    SceneManager.LoadScene("Login");
                                    break;
                                case 2:
                                    Debug.Log("Database Error");
                                    break;
                                default:
                                    Debug.Log("Invalid errorId!");
                                    break;
                            }
                        }
                        break;
                    }
                    case FriendTags.GetAllFriends:
                    {
                        using (var reader = message.GetReader())
                        {
                            var onlineFriends = reader.ReadStrings();
                            var offlineFriends = reader.ReadStrings();
                            var openRequests = reader.ReadStrings();
                            var unansweredRequests = reader.ReadStrings();

                            foreach (var friend in onlineFriends)
                            {
                                ChatManager.Instance.ServerMessage(friend + " is online.", MessageType.Info);
                            }

                            onSuccessfulGetAllFriends?.Invoke(onlineFriends, offlineFriends, openRequests, unansweredRequests);
                        }
                        break;
                    }
                    case FriendTags.GetAllFriendsFailed:
                    {
                        ChatManager.Instance.ServerMessage("Failed to load Friendlist!", MessageType.Error);

                        using (var reader = message.GetReader())
                        {
                            if (reader.Length != 1)
                            {
                                Debug.LogWarning("Invalid RemoveFriend Error data received.");
                                return;
                            }

                            switch (reader.ReadByte())
                            {
                                case 1:
                                    Debug.Log("You're not logged in!");
                                    SceneManager.LoadScene("Login");
                                    break;
                                case 2:
                                    Debug.Log("Database Error");
                                    break;
                                default:
                                    Debug.Log("Invalid errorId!");
                                    break;
                            }
                        }
                        break;
                    }
                    case FriendTags.FriendLoggedIn:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            ChatManager.Instance.ServerMessage(friendName + " is online.", MessageType.Info);

                            onFriendLogin?.Invoke(friendName);
                        }
                        break;
                    }
                    case FriendTags.FriendLoggedOut:
                    {
                        using (var reader = message.GetReader())
                        {
                            var friendName = reader.ReadString();
                            ChatManager.Instance.ServerMessage(friendName + " is offline.", MessageType.Info);

                            onFriendLogout?.Invoke(friendName);
                        }
                        break;
                    }
                }
            }
        }
    }
}
