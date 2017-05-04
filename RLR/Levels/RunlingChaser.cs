using System.Collections.Generic;
using Assets.Scripts.Drones;
using Assets.Scripts.RLR.GenerateMap;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class RunlingChaser : MonoBehaviour
    {
        private DeathTrigger _deathTrigger;
        public GenerateMapRLR GenerateMap;
        public DroneFactory DroneFactory;

        private bool _createdInstance;
        private int[] _spawnChaser = {1};
        private int[] _destroyChaser = { 2 };

        private bool[,] _reachedChaserPlatform;
        private List<GameObject> _safeZones;
        private GameObject _chaser;
        

        void Awake()
        {
            _createdInstance = false;
            SetChaserPlatforms(new[] {1,3}, new[] { 2,4 });
        }
        
        public void SetChaserPlatforms(int[] spawnChaser, int[] destroyChaser)
        {
            _spawnChaser = spawnChaser;
            _destroyChaser = destroyChaser;
            _reachedChaserPlatform = new bool[spawnChaser.Length,2];

            for (var i = 0; i < spawnChaser.Length; i++)
            {
                _reachedChaserPlatform[i, 0] = false;
                _reachedChaserPlatform[i, 1] = false;
            }
        }

        public void GetTriggerInstance(GameObject player)
        {
            _deathTrigger = player.GetComponent<DeathTrigger>();
            _createdInstance = true;
        }

        public void GetSaveZones()
        {
            _safeZones = GenerateMap.GetSafeZones();
            _safeZones.Reverse();
        }
        
        private int? GetPlatformIndex(GameObject currentSaveZone)
        {
            for (var i = 0; i < _safeZones.Count; i++)
            {
                if (currentSaveZone == _safeZones[i])
                {
                    //Debug.Log(i);
                    return i;
                }
            }
            return null;
        }

        private void IsChaser(GameObject currentSaveZone)
        {
            int? index = GetPlatformIndex(currentSaveZone);
            if (index == null) return;

            for(var i = 0; i < _spawnChaser.Length; i++)
            {
                if (_spawnChaser[i] == index.Value && !_reachedChaserPlatform[i, 0])
                {
                    _chaser = DroneFactory.SpawnDrones(new ChaserDrone(7f, 1f, Color.yellow, _deathTrigger.transform.parent.gameObject));
                    if (_spawnChaser[i] != 0)
                    {
                        _chaser.transform.position = _safeZones[_spawnChaser[i] - 1].transform.position + new Vector3(0,0.6f,0);
                    }
                    _chaser.tag = "Strong Enemy";
                    _reachedChaserPlatform[i, 0] = true;
                }
                if (_destroyChaser[i] == index.Value && !_reachedChaserPlatform[i,1])
                {
                    DroneFactory.StopCoroutine("MoveChaser");
                    GameObject.Destroy(_chaser);
                    _reachedChaserPlatform[i, 1] = true;
                }
            }
        }

        
        void Update()
        {
            if (_deathTrigger.EnterSaveZone && _createdInstance)
            {
                IsChaser(_deathTrigger.SaveZone);
                _deathTrigger.EnterSaveZone = false;
            }
        }
    }
}
