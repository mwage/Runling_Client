using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Drones;
using Assets.Scripts.Players;
using Assets.Scripts.RLR.GenerateMap;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class RunlingChaser : MonoBehaviour
    {
        private PlayerTrigger _playerTrigger;
        public GenerateMapRLR GenerateMap;
        public DroneFactory DroneFactory;

        public GameObject ChaserText;
        private bool _createdInstance;
        private int[] _spawnChaser = {1};
        private int[] _destroyChaser = { 2 };
        private IDrone _chaserBase;
        private bool[,] _reachedChaserPlatform;
        private List<GameObject> _safeZones;
        private GameObject[] _chaser;

        void Awake()
        {
            _createdInstance = false;
        }

        public void SetChaserPlatforms(IDrone chaserBase, int[] spawnChaser = null, int[] destroyChaser = null)
        {
            _spawnChaser = spawnChaser;
            _destroyChaser = destroyChaser;
            _chaserBase = chaserBase;
            
            if (_spawnChaser == null || _destroyChaser == null) return;

            _reachedChaserPlatform = new bool[_spawnChaser.Length,2];
            _chaser = new GameObject[_spawnChaser.Length];

            for (var i = 0; i < _spawnChaser.Length; i++)
            {
                _reachedChaserPlatform[i, 0] = false;
                _reachedChaserPlatform[i, 1] = false;
            }
        }

        public void GetTriggerInstance(GameObject player)
        {
            _playerTrigger = player.GetComponent<PlayerTrigger>();
            _createdInstance = true;
        }

        public void GetSafeZones()
        {
            _safeZones = GenerateMap.GetSafeZones();
            _safeZones.Reverse();
        }

        private int? GetPlatformIndex(GameObject currentSafeZone)
        {
            if (_safeZones.Contains(currentSafeZone))
                return _safeZones.IndexOf(currentSafeZone);

            return null;
        }

        private void IsChaser(GameObject currentSafeZone)
        {
            if (_spawnChaser == null || _destroyChaser == null) return;

            var index = GetPlatformIndex(currentSafeZone);
            if (index == null) return;

            for (var i = 0; i < _spawnChaser.Length; i++)
            {
                if (_spawnChaser[i] == index.Value && !_reachedChaserPlatform[i, 0])
                {
                    _chaser[i] = DroneFactory.SpawnDrones(_chaserBase);
                    if (_spawnChaser[i] != 0)
                    {
                        _chaser[i].transform.position = _safeZones[_spawnChaser[i] - 1].transform.position +
                                                        new Vector3(0, 0.6f, 0);
                    }
                    _chaser[i].tag = "Strong Enemy";
                    _reachedChaserPlatform[i, 0] = true;
                    _safeZones[_destroyChaser[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(true);
                    DroneFactory.StartCoroutine(SpawnChaserText(3f));
                }
                if (_destroyChaser[i] == index.Value && !_reachedChaserPlatform[i, 1])
                {
                    DroneFactory.StopCoroutine("MoveChaser");
                    Destroy(_chaser[i]);
                    _reachedChaserPlatform[i, 1] = true;
                    _safeZones[_destroyChaser[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(false);
                    DroneFactory.StartCoroutine(DestroyChaserText(3f));
                }
            }
        }

        IEnumerator SpawnChaserText(float duration)
        {
            var chaserText = Instantiate(ChaserText, GameObject.Find("Canvas").transform);
            chaserText.GetComponent<TextMeshProUGUI>().text = "Warning: Chaser Drone!";
            yield return new WaitForSeconds(duration);
            Destroy(chaserText);
        }

        IEnumerator DestroyChaserText(float duration)
        {
            var chaserText = Instantiate(ChaserText, GameObject.Find("Canvas").transform);
            chaserText.GetComponent<TextMeshProUGUI>().text = "Chaser Drone destroyed!";
            yield return new WaitForSeconds(duration);
            Destroy(chaserText);
        }

        void Update()
        {
            if (_playerTrigger.EnterSaveZone && _createdInstance)
            {
                IsChaser(_playerTrigger.SaveZone);
                _playerTrigger.EnterSaveZone = false;
            }
        }
    }
}
