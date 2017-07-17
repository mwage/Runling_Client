using System.Collections;
using System.Collections.Generic;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using Launcher;
using TMPro;
using UnityEngine;

namespace RLR.Levels
{
    public class RunlingChaser : MonoBehaviour
    {
        public DroneFactory DroneFactory;
        public GameObject ChaserText;
        public CheckSafeZones CheckSafeZones;

        private int[] _chaserSpawnPlatformIdxs;
        private int[] _chaserDestroyPlatformIdxs;
        private int[] _chaserStartPosition;
        private IDrone _chaserBase;
        private bool[,] _reachedChaserPlatform;
        private List <GameObject> _chasers;
        private IDrone _iDrone;
        private IPattern _pattern;

        /// <summary>
        /// Sets plaforms where chasers have start and destory position and which platforms spawn them.
        /// </summary>
        public void SetChaserPlatforms(IDrone chaserBase, int[] chaserSpawnPlatformIndex = null, int[] chaserDestroyPlatformIndex = null, int[] chaserStartPlatformIndex = null, IPattern pattern = null, IDrone iDrone = null) 
        {
            _chaserSpawnPlatformIdxs = chaserSpawnPlatformIndex;
            _chaserDestroyPlatformIdxs = chaserDestroyPlatformIndex;
            _chaserBase = chaserBase;
            _pattern = pattern;
            _iDrone = iDrone ?? new DefaultDrone(7, 1, DroneColor.Cyan);


            if (_chaserSpawnPlatformIdxs == null || _chaserDestroyPlatformIdxs == null) return;

            _reachedChaserPlatform = new bool[_chaserSpawnPlatformIdxs.Length, 2];
            _chasers = new List<GameObject>();

            for (var i = 0; i < _chaserSpawnPlatformIdxs.Length; i++)
            {
                _reachedChaserPlatform[i, 0] = false;
                _reachedChaserPlatform[i, 1] = false;
            }

            if (chaserStartPlatformIndex == null || chaserStartPlatformIndex.Length != _chaserSpawnPlatformIdxs.Length)
            {
                _chaserStartPosition = new int[_chaserSpawnPlatformIdxs.Length];
                for (var i = 0; i < _chaserSpawnPlatformIdxs.Length; i++)
                {
                    _chaserStartPosition[i] = _chaserSpawnPlatformIdxs[i] - 1;
                }
            }
            else
            {
                _chaserStartPosition = chaserStartPlatformIndex;
            }
        }


        public void CreateOrDestroyChaserIfNeed(GameObject currentSafeZone)
        {
            var safeZones = GameControl.MapState.SafeZones;
            if (_chaserSpawnPlatformIdxs == null || _chaserDestroyPlatformIdxs == null) return;

            var platformIndex = CheckSafeZones.GetPlatformIndex(currentSafeZone);
            if (platformIndex == null) return;

            for (var i = 0; i < _chaserSpawnPlatformIdxs.Length; i++)
            {
                if (_chaserSpawnPlatformIdxs[i] == platformIndex.Value && !_reachedChaserPlatform[i, 0] && (i == 0 || _reachedChaserPlatform[i-1, 0]))
                {
                    _chasers.AddRange(DroneFactory.SpawnDrones(_chaserBase)); // you add few objects (addrange), but you actyally add only one each time in loop, make it before loop or change to .Add()
                    
                    if (_chaserSpawnPlatformIdxs[i] != 0) // sets start position of chaser
                    {
                        _chasers[i].transform.position = safeZones[_chaserStartPosition[i]].transform.position + safeZones[_chaserStartPosition[i]].transform.rotation *
                                                        new Vector3(safeZones[_chaserStartPosition[i]].transform.Find("VisibleObjects/Ground").transform.localScale.x / 2 + _chaserBase.Size / 2 + 0.5f, 0.4f, 0);
                    }
                    if (_pattern != null)
                    {
                        DroneFactory.AddPattern(_pattern, _chasers[i], _iDrone);
                    }
                    _chasers[i].tag = "Strong Enemy";
                    _reachedChaserPlatform[i, 0] = true;
                    safeZones[_chaserDestroyPlatformIdxs[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(true);
                    DroneFactory.StartCoroutine(SpawnChaserText(3f));
                }
                if (_chaserDestroyPlatformIdxs[i] == platformIndex.Value && !_reachedChaserPlatform[i, 1] && _reachedChaserPlatform[i, 0])
                {
                    Destroy(_chasers[i]);
                    _reachedChaserPlatform[i, 1] = true;
                    safeZones[_chaserDestroyPlatformIdxs[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(false);
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
