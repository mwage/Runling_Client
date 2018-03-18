using Client.Scripts.Launcher;
using Client.Scripts.Network;
using DarkRift;
using DarkRift.Client;
using Game.Scripts.Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Scripts.SLA.MultiPlayer
{
    public class SetupNetworkSLA : MonoBehaviour
    {
        public GameObject Game;
        public GameObject Voting;
        public GameObject NetworkManager;

        ////////////////////////////// For faster testing only! /////////////////////////
        private bool _testMode;
        /////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {            
            ////////////////////////////// For faster testing only! /////////////////////////
            _testMode = false;

            if (!MainClient.Instance.Connected)
            {
                GameControl.GameState.Solo = true;
            }
            /////////////////////////////////////////////////////////////////////////////////
            
            if (GameControl.GameState.Solo)
            {
                Voting.SetActive(false);
                Game.SetActive(true);
                gameObject.SetActive(false);
                return;
            }


            if (GameClient.Instance.Connect(MainClient.Instance.GameServerIp, MainClient.Instance.GameServerPort,
                MainClient.Instance.GameServerIpVersion))
            {
                Debug.Log("Connected to game server");
                NetworkManager.SetActive(true);
                Voting.SetActive(true);

                GameClient.Instance.MessageReceived += OnDataHandler;

                if (!_testMode)
                {
                    // Identify
                    using (var writer = DarkRiftWriter.Create())
                    {
                        writer.Write(MainClient.Instance.Id);

                        using (var msg = Message.Create(GameServerTags.IdentifyPlayer, writer))
                        {
                            GameClient.Instance.SendMessage(msg, SendMode.Reliable);
                        }
                    }
                }
                ////////////////////////////// For faster testing only! /////////////////////////
                else
                {
                    using (var msg = Message.CreateEmpty(GameServerTags.TestModeSLA))
                    {
                        GameClient.Instance.SendMessage(msg, SendMode.Reliable);
                    }
                }
                /////////////////////////////////////////////////////////////////////////////////
            }
            else
            {
                Debug.Log("Failed to connect to game server");
                Debug.Log("TODO: Insert proper failed to connect logic!");
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.GameServer || message.Tag >= Tags.TagsPerPlugin * (Tags.GameServer + 1))
                    return;

                switch (message.Tag)
                {
                    case GameServerTags.IdentifyPlayer:
                    {
                        using (var reader = message.GetReader())
                        {
                            while (reader.Position < reader.Length)
                            {
                                var player = reader.ReadSerializable<global::Game.Scripts.Network.Data.Player>();
                                GameClient.Instance.Players.Add(player);
                                Debug.Log(player.Name + " has joined the game");
                            }
                        }
                        break;
                    }
                    case GameServerTags.IdentifyPlayerFailed:
                    {
                        Debug.Log("Failed to identify player");
                        SceneManager.LoadScene("MainMenu");
                        break;
                    }
                    case GameServerTags.PlayerJoined:
                    {
                        using (var reader = message.GetReader())
                        {
                            var player = reader.ReadSerializable<global::Game.Scripts.Network.Data.Player>();
                            GameClient.Instance.Players.Add(player);
                            Debug.Log(player.Name + " has joined the game");
                        }
                        break;
                    }
                }
            }
        }
    }
}
