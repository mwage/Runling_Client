using System.Collections.Generic;
using Launcher;
using Players;
using RLR.GenerateMap;
using UnityEngine;

namespace RLR.Levels
{
    public class CheckSafeZones : MonoBehaviour
    {
        private PlayerTrigger _playerTrigger;
        public MapGeneratorRLR MapGenerator;
        public RunlingChaser RunlingChaser;
        public ScoreRLR ScoreRLR;

        private List<GameObject> _safeZones;


        private void Awake()
        {
        }

        public void SetUpPlayerTrigger()
        {
            _playerTrigger = GameControl.PlayerState.Player.transform.Find("Trigger").gameObject.GetComponent<PlayerTrigger>();
        }

        public int? GetPlatformIndex(GameObject currentSafeZone)
        {
            if (GameControl.MapState.SafeZones.Contains(currentSafeZone))
                return GameControl.MapState.SafeZones.IndexOf(currentSafeZone);

            return null;
        }

        private void Update()
        {
            if (_playerTrigger == null) return;
            if (_playerTrigger.EnteredOnNewPlatform)
            {
                RunlingChaser.CreateOrDestroyChaserIfNeed(_playerTrigger.LastVisitedSafeZone);

                ScoreRLR.AddScore(_playerTrigger.LastVisitedSafeZone, _safeZones);
                _playerTrigger.EnteredOnNewPlatform = false;
            }
        }

        public void CreateOrDestroyChaserIfNeed()
        {
            RunlingChaser.CreateOrDestroyChaserIfNeed(_playerTrigger.LastVisitedSafeZone);
        }
    }
}