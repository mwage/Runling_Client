using Launcher;
using Network;
using Players;
using UnityEngine;

namespace SLA
{
    public class DeathSLA : MonoBehaviour
    {
        private LevelManagerSLA _levelManager;
        private ControlSLA _controlSLA;
        private ScoreSLA _score;

        private void Awake()
        {
            _levelManager = GetComponent<LevelManagerSLA>();
            _controlSLA = GetComponent<ControlSLA>();
            _score = GetComponent<ScoreSLA>();

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
            playerManager.Model.SetActive(false);
            
            if (GameControl.GameState.Solo)
            {
                SetScores(playerManager);
                playerManager.DestroyChaser();
                _levelManager.EndLevel(1);
            }
            else if (playerManager.Player.Id == GameClient.Instance.Id)
            {
                // TODO: trigger highscore from Serverside via comparison with DB.
                SetScores(playerManager);
            }
        }

        private void SetScores(PlayerManager playerManager)
        {
            _score.SetHighScore(playerManager);

            if (GameControl.GameState.SetGameMode != GameMode.Practice)
            {
                _score.Scores[playerManager].ScoresCurrentGame[_controlSLA.CurrentLevel - 1] = _score.Scores[playerManager].CurrentScore;
            }
        }
    }
}
