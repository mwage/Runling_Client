using DarkRift;
using DarkRift.Client;
using Launcher;
using Network;
using Network.DarkRiftTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SLA.Network
{
    public class NetworkManagerSLA : MonoBehaviour
    {
        public GameObject Game;
        public GameObject Voting;
        public GameClient GameClient;

        private void Awake()
        {
            GameClient = GetComponent<GameClient>();
            if (GameControl.GameState.Solo)
            {
                Voting.SetActive(false);
                Game.SetActive(true);
                gameObject.SetActive(false);
                return;
            }

            Voting.SetActive(true);

            if (GameClient.Connect(MainClient.Instance.GameServerIp, MainClient.Instance.GameServerPort,
                MainClient.Instance.GameServerIpVersion))
            {
                Debug.Log("Connected to game server");

                GameClient.MessageReceived += OnDataHandler;
                
                // Identify
                var writer = new DarkRiftWriter();
                writer.Write(MainClient.Instance.Id);
                GameClient.SendMessage(new TagSubjectMessage(Tags.GameServer, GameServerSubjects.IdentifyPlayer, writer), SendMode.Reliable);
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
                Debug.Log("TODO: Successfully identified message via chat");
            }
            else if (message.Subject == GameServerSubjects.IdentifyPlayerFailed)
            {
                Debug.Log("Failed to identify player");
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
