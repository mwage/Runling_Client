using Launcher;
using Network;
using UnityEngine;

namespace SLA.Network
{
    public class NetworkSLA : MonoBehaviour
    {
        public GameObject Game;
        public GameObject Voting;
        private GameClient _gameClient;

        private void Awake()
        {
            if (GameControl.GameState.Solo)
            {
                Voting.SetActive(false);
                Game.SetActive(true);
                gameObject.SetActive(false);
                return;
            }

            _gameClient.Connect(MainClient.Instance.GameServerIp, MainClient.Instance.GameServerPort, MainClient.Instance.GameServerIpVersion);
        }
    }
}
