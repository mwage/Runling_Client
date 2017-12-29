using DarkRift;
using DarkRift.Client;
using Launcher;
using Network;
using Network.DarkRiftTags;
using Network.Synchronization.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLA.Network
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
            GameControl.GameState.Solo = true;
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
                    var writer = new DarkRiftWriter();
                    writer.Write(MainClient.Instance.Id);
                    GameClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.IdentifyPlayer, writer), SendMode.Reliable);
                }
                ////////////////////////////// For faster testing only! /////////////////////////
                else
                {
                    GameClient.Instance.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.TestModeSLA, new DarkRiftWriter()), SendMode.Reliable);
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
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.GameServer)
                return;

            if (message.Subject == GameServerSubjects.IdentifyPlayer)
            {
                var reader = message.GetReader();
                while (reader.Position < reader.Length)
                {
                    var player = reader.ReadSerializable<Player>();
                    GameClient.Instance.Players.Add(player);
                    Debug.Log(player.Name + " has joined the game");
                }
            }
            else if (message.Subject == GameServerSubjects.IdentifyPlayerFailed)
            {
                Debug.Log("Failed to identify player");
                SceneManager.LoadScene("MainMenu");
            }
            else if (message.Subject == GameServerSubjects.PlayerJoined)
            {
                var reader = message.GetReader();
                var player = reader.ReadSerializable<Player>();
                GameClient.Instance.Players.Add(player);
                Debug.Log(player.Name + " has joined the game");
            }
        }
    }
}
