using Launcher;
using MP.TSGame.Players;
using TrueSync;

namespace MP.TSGame.SLA
{
    public class DeathSLA : TrueSyncBehaviour
    {
        public void Death(PlayerManager playerManager, ScoreSLA score)
        {
            playerManager.CheckIfDead = false;
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(false);
            playerManager.Trigger.SetActive(false);
            playerManager.DestroyChaser();

            // Check if highscore and save it
            if (GameControl.GameState.SetGameMode != GameMode.Practice && playerManager.owner == TrueSyncManager.LocalPlayer)
            {
                score.SetHighScore();
            }
        }
    }
}
