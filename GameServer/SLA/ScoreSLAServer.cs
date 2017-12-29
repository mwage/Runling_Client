using Players;
using SLA;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server.Scripts.SLA
{
    public class ScoreSLAServer : MonoBehaviour
    {
        public Dictionary<PlayerManager, ScoreDataSLA> Scores { get; } = new Dictionary<PlayerManager, ScoreDataSLA>();

        private ControlSLAServer _controlSLA;
        private readonly List<ScoreDataSLA> _playersToUpdate = new List<ScoreDataSLA>();

        private void Awake()
        {
            _controlSLA = GetComponent<ControlSLAServer>();
        }

        public void StartScore()
        {
            StartCoroutine(AddScore());
        }

        private IEnumerator AddScore()
        {
            yield return new WaitForSeconds(0.25f);
            
            while (_controlSLA.PlayerManagers.Values.Any(pM => !pM.IsDead))
            {
                foreach (var playerManager in _controlSLA.PlayerManagers.Values)
                {
                    if (!playerManager.IsDead)
                    {
                        Scores[playerManager].IncrementScore(2);
                        _playersToUpdate.Add(Scores[playerManager]);
                    }
                }

                NetworkManagerSLAServer.UpdateScore(_playersToUpdate);
                _playersToUpdate.Clear();

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
