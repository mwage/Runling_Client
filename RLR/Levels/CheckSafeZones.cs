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
        public GenerateMapRLR GenerateMap;
        public RunlingChaser RunlingChaser;
        public ScoreRLR ScoreRLR;
        public bool[] VisitedSafeZone;

        private List<GameObject> _safeZones;


        private void Awake()
        {
        }

        public void SetUpPlayerTrigger()
        {
            _playerTrigger = GameControl.PlayerState.Player.transform.Find("Trigger").gameObject.GetComponent<PlayerTrigger>();
        }

        public void GetSafeZones()
        {
            _safeZones = GenerateMap.GetSafeZones();
            VisitedSafeZone = new bool[_safeZones.Count];
        }

        public int? GetPlatformIndex(GameObject currentSafeZone, List<GameObject> safeZones)
        {
            if (safeZones.Contains(currentSafeZone))
                return safeZones.IndexOf(currentSafeZone);

            return null;
        }

        private void Update()
        {
            if (_playerTrigger == null) return;
            if (_playerTrigger.EnteredOnNewPlatform)
            {
                RunlingChaser.CreateOrDestroyChaserIfNeed(_playerTrigger.LastVisitedSafeZone, _safeZones);

                ScoreRLR.AddScore(_playerTrigger.LastVisitedSafeZone, _safeZones);
                _playerTrigger.EnteredOnNewPlatform = false;
            }
        }
    }
}