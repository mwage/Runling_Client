using Launcher;
using Players;
using UnityEngine;

namespace SLA
{
    public class DeathSLA : MonoBehaviour
    {
        private ScoreSLA _score;
        private LevelManagerSLA _levelManager;

        private void Awake()
        {
            _score = GetComponent<ScoreSLA>();
            _levelManager = GetComponent<LevelManagerSLA>();
            PlayerTrigger.onPlayerDeath += Death;
        }

        public void Death(PlayerManager playerManager)
        {
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(false);
            playerManager.DestroyChaser();

            // Check if highscore and save it
            if (GameControl.GameState.SetGameMode != GameMode.Practice)
                _score.SetHighScore();

            _levelManager.EndLevel(1);
        }

        private void OnDestroy()
        {
            PlayerTrigger.onPlayerDeath -= Death;
        }
    }
}
