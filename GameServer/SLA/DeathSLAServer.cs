using System.Linq;
using Launcher;
using Players;
using Server.Scripts.Synchronization;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class DeathSLAServer : MonoBehaviour
    {
        private LevelManagerSLAServer _levelManager;
        private ControlSLAServer _controlSLA;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerSLAServer>();
            _controlSLA = GetComponent<ControlSLAServer>();

            PlayerTrigger.onPlayerDeath += Death;
        }

        private void OnDestroy()
        {
            PlayerTrigger.onPlayerDeath -= Death;
        }

        public void Death(PlayerManager playerManager)
        {
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            SyncPlayerServer.PlayerDied(playerManager.Player.Id);

            ServerManager.Instance.Server.Dispatcher.InvokeAsync(() =>
            {
                playerManager.Model.SetActive(false);
                playerManager.DestroyChaser();
            });


            // Check if highscore and save it
            if (_controlSLA.GameMode != GameMode.Practice)
            {
                // _score.SetHighScore();
            }

            // TODO: Send info to the players

            if (_controlSLA.PlayerManagers.Values.Any(player => !player.IsDead))
                return;

            _levelManager.EndLevel(1);
        }
    }
}
