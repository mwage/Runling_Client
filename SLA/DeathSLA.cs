using Launcher;
using Players;
using UnityEngine;

namespace SLA
{
    public class DeathSLA : MonoBehaviour
    {
        private ScoreSLA _score;

        private void Awake()
        {
            _score = GetComponent<ScoreSLA>();
        }

        public void Death(PlayerManager playerManager)
        {
            playerManager.CheckIfDead = false;
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(false);
            playerManager.DestroyChaser();

            // Check if highscore and save it
            if (GameControl.GameState.SetGameMode != GameMode.Practice)
                _score.SetHighScore();
        }
    }
}
