using System.Collections;
using System.Collections.Generic;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using Launcher;
using Players;
using RLR.GenerateMap;
using TMPro;
using UnityEngine;

namespace RLR.Levels
{
    public class RunlingChaser : MonoBehaviour
    {
        public DroneFactory DroneFactory;
        public GameObject ChaserText;
        public CheckSafeZones CheckSafeZones;

        private int[] _spawnChaser;
        private int[] _destroyChaser;
        private int[] _chaserStartPosition;
        private IDrone _chaserBase;
        private bool[,] _reachedChaserPlatform;
        private List <GameObject> _chaser;
        private IDrone _iDrone;
        private IPattern _pattern;


        public void SetChaserPlatforms(IDrone chaserBase, int[] spawnChaser = null, int[] destroyChaser = null, int[] chaserStartPosition = null, IPattern pattern = null, IDrone iDrone = null) 
        {
            _spawnChaser = spawnChaser;
            _destroyChaser = destroyChaser;
            _chaserBase = chaserBase;
            _pattern = pattern;
            _iDrone = iDrone ?? new DefaultDrone(7, 1, DroneColor.Cyan);


            if (_spawnChaser == null || _destroyChaser == null) return;

            _reachedChaserPlatform = new bool[_spawnChaser.Length, 2];
            _chaser = new List<GameObject>();

            for (var i = 0; i < _spawnChaser.Length; i++)
            {
                _reachedChaserPlatform[i, 0] = false;
                _reachedChaserPlatform[i, 1] = false;
            }

            if (chaserStartPosition == null || chaserStartPosition.Length != _spawnChaser.Length)
            {
                _chaserStartPosition = new int[_spawnChaser.Length];
                for (var i = 0; i < _spawnChaser.Length; i++)
                {
                    _chaserStartPosition[i] = _spawnChaser[i] - 1;
                }
            }
            else
            {
                _chaserStartPosition = chaserStartPosition;
            }
        }


        public void IsChaser(GameObject currentSafeZone, List<GameObject> safeZones)
        {
            if (_spawnChaser == null || _destroyChaser == null) return;

            var index = CheckSafeZones.GetPlatformIndex(currentSafeZone, safeZones);
            if (index == null) return;
            for (var i = 0; i < _spawnChaser.Length; i++)
            {
                if (_spawnChaser[i] == index.Value && !_reachedChaserPlatform[i, 0] && (i == 0 || _reachedChaserPlatform[i-1, 0]))
                {
                    _chaser.AddRange(DroneFactory.SpawnDrones(_chaserBase));
                    
                    if (_spawnChaser[i] != 0)
                    {
                        _chaser[i].transform.position = safeZones[_chaserStartPosition[i]].transform.position + safeZones[_chaserStartPosition[i]].transform.rotation *
                                                        new Vector3(safeZones[_chaserStartPosition[i]].transform.Find("VisibleObjects/Ground").transform.localScale.x / 2 + _chaserBase.Size / 2 + 0.5f, 0.4f, 0);
                    }
                    if (_pattern != null)
                    {
                        DroneFactory.AddPattern(_pattern, _chaser[i], _iDrone);
                    }
                    _chaser[i].tag = "Strong Enemy";
                    _reachedChaserPlatform[i, 0] = true;
                    safeZones[_destroyChaser[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(true);
                    DroneFactory.StartCoroutine(SpawnChaserText(3f));
                }
                if (_destroyChaser[i] == index.Value && !_reachedChaserPlatform[i, 1] && _reachedChaserPlatform[i, 0])
                {
                    Destroy(_chaser[i]);
                    _reachedChaserPlatform[i, 1] = true;
                    safeZones[_destroyChaser[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(false);
                    DroneFactory.StartCoroutine(DestroyChaserText(3f));
                }
            }
        }

        private IEnumerator SpawnChaserText(float duration)
        {
            var chaserText = Instantiate(ChaserText, GameObject.Find("Canvas").transform);
            chaserText.GetComponent<TextMeshProUGUI>().text = "Warning: Chaser Drone!";
            yield return new WaitForSeconds(duration);
            Destroy(chaserText);
        }

        private IEnumerator DestroyChaserText(float duration)
        {
            var chaserText = Instantiate(ChaserText, GameObject.Find("Canvas").transform);
            chaserText.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 50);
            chaserText.GetComponent<TextMeshProUGUI>().text = "Chaser Drone destroyed!";
            yield return new WaitForSeconds(duration);
            Destroy(chaserText);
        }


    }
}
