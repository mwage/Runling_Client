using DarkRift;
using DarkRift.Client;
using Launcher;
using Network;
using Network.DarkRiftTags;
using Players;
using UnityEngine;

namespace SLA.Network
{
    public class NetworkManagerSLA : MonoBehaviour
    {
        [SerializeField] private InitializeGameSLA _initializeGame;

        private ControlSLA _controlSLA;

        private void Awake()
        {
            if (GameControl.GameState.Solo)
                return;

            _controlSLA = _initializeGame.gameObject.GetComponent<ControlSLA>();

            GameClient.Instance.MessageReceived += OnDataHandler;
        }

        private void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            var message = e.Message as TagSubjectMessage;

            if (message == null || message.Tag != Tags.SLA)
                return;

            // Initialize Players
            if (message.Subject == SLASubjects.InitializePlayers)
            {
                foreach (var player in GameClient.Instance.Players)
                {
                    PlayerManager playerManager;

                    if (player.Id == GameClient.Instance.Id)
                    {
                        Debug.Log("Initializing me");
                        playerManager = _initializeGame.InitializePlayer(player);
                    }
                    else
                    {
                        Debug.Log("Initializing another player");
                        playerManager = _initializeGame.InitializeOtherPlayer(player);
                    }
                    _controlSLA.PlayerManagers[playerManager.Player.Id] = playerManager;
                }
            }
            else if (message.Subject == SLASubjects.PrepareLevel)
            {
                var reader = message.GetReader();
                _controlSLA.CurrentLevel = reader.ReadByte();
                _initializeGame.PrepareLevel();
            }
            else if (message.Subject == SLASubjects.StartLevel)
            {
                foreach (var playerManager in _controlSLA.PlayerManagers.Values)
                {
                    _initializeGame.StartLevel(playerManager);
                }
            }
            else if (message.Subject == SLASubjects.HidePanels)
            {
                _initializeGame.HidePanels();
            }
        }
    }
}
