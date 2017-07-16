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

        private bool _createdInstance;
        private List<GameObject> _safeZones;


        private void Awake()
        {
            _createdInstance = false;
        }

        public void GetTriggerInstance()
        {
            _playerTrigger = GameControl.State.Player.transform.Find("Trigger").gameObject.GetComponent<PlayerTrigger>();
            _createdInstance = true;
        }

        public void GetSafeZones()
        {
            _safeZones = GenerateMap.GetSafeZones();
            _safeZones.Reverse();
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
            if (_playerTrigger.EnterSaveZone && _createdInstance)
            {
                RunlingChaser.IsChaser(_playerTrigger.SaveZone, _safeZones);
                ScoreRLR.AddScore(_playerTrigger.SaveZone, _safeZones);
                _playerTrigger.EnterSaveZone = false;
            }
        }
    }
}