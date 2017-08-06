using Characters.Bars;
using Launcher;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace Players
{
    public class PlayerTrigger : MonoBehaviour
    {
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
                GameControl.GameState.FinishedLevel = true;
            }
            
            // Safety Death Trigger
            if (((other.CompareTag("Enemy") && !_playerManager.IsSafe || other.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                _playerManager.IsDead = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // DeathTrigger
            if (((other.CompareTag("Enemy") && !_playerManager.IsSafe || other.CompareTag("Strong Enemy"))
                 && !_playerManager.IsInvulnerable) && !_playerManager.GodModeActive)
            {
                _playerManager.IsDead = true;
            }
        }
    }
}