using Client.Scripts.Launcher;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class PlayerTrigger : MonoBehaviour
    {
        public delegate void PlayerDeathEventHandler(PlayerManager playerManager);
        public delegate void FinishedLevelEventHandler();

        public static event PlayerDeathEventHandler onPlayerDeath;
        public static event FinishedLevelEventHandler onFinishedLevel;
    

        private PlayerManager _playerManager;

        private void Awake()
        {
            _playerManager = transform.parent.GetComponent<PlayerManager>();
        }


        private void OnTriggerStay(Collider other)
        {
            // Enter Finishzone
            if (other.CompareTag("Finish"))
            {
                if (!GameControl.GameState.FinishedLevel)
                {
                    GameControl.GameState.FinishedLevel = true;
                    onFinishedLevel?.Invoke();
                }
            }
            
            // Safety Death Trigger
            if (((other.CompareTag("Enemy") && !_playerManager.IsSafe || other.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                if (!_playerManager.IsDead)
                {
                    _playerManager.IsDead = true;
                    onPlayerDeath?.Invoke(_playerManager);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // DeathTrigger
            if (((other.CompareTag("Enemy") && !_playerManager.IsSafe || other.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                if (!_playerManager.IsDead)
                {
                    _playerManager.IsDead = true;
                    onPlayerDeath?.Invoke(_playerManager);
                }
            }
        }
    }
}