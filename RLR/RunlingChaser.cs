using System.Collections;
using System.Collections.Generic;
using Drones.DroneTypes;
using Drones.Pattern;
using Launcher;
using Players;
using TMPro;
using UnityEngine;

namespace RLR
{
    public class RunlingChaser : MonoBehaviour
    {
        public LevelManagerRLR LevelManager;
        public GameObject ChaserText;

        private int[] _chaserSpawnPlatformIdxs;
        private int[] _chaserDestroyPlatformIdxs;
        private int[] _chaserStartPosition;
        private IDrone _chaserBase;

        private IDrone _iDrone;
        private IPattern _pattern;


        /// <summary>
        /// Sets plaforms where chasers have start and destory position and which platforms spawn them.
        /// </summary>
        public void SetChaserPlatforms(IDrone chaserBase, int[] chaserSpawnPlatformIndex = null,
            int[] chaserDestroyPlatformIndex = null, int[] chaserStartPlatformIndex = null, IPattern pattern = null,
            IDrone iDrone = null)
        {
            _chaserSpawnPlatformIdxs = chaserSpawnPlatformIndex;
            _chaserDestroyPlatformIdxs = chaserDestroyPlatformIndex;
            _chaserBase = chaserBase;
            _pattern = pattern;
            _iDrone = iDrone ?? new DefaultDrone(7, 1, DroneColor.Cyan);

            if (_chaserSpawnPlatformIdxs == null || _chaserDestroyPlatformIdxs == null)
                return;

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

        public void InitializeChaserPlatforms(SafeZoneManager safeZoneManager)
        {
            if (_chaserSpawnPlatformIdxs == null || _chaserDestroyPlatformIdxs == null)
                return;

            safeZoneManager.ReachedChaserPlatform = new bool[_chaserSpawnPlatformIdxs.Length, 2];
            safeZoneManager.Chasers = new List<GameObject>();

            for (var i = 0; i < _chaserSpawnPlatformIdxs.Length; i++)
            {
                safeZoneManager.ReachedChaserPlatform[i, 0] = false;
                safeZoneManager.ReachedChaserPlatform[i, 1] = false;
            }
        }

        public void CreateOrDestroyChaserIfNeed(GameObject currentSafeZone, PlayerManager playerManager, SafeZoneManager safeZoneManager, int platformIndex)
        {
            var safeZones = GameControl.GameState.SafeZones;
            if (_chaserSpawnPlatformIdxs == null || _chaserDestroyPlatformIdxs == null) return;

            for (var i = 0; i < _chaserSpawnPlatformIdxs.Length; i++)
            {
                if (_chaserSpawnPlatformIdxs[i] == platformIndex && !safeZoneManager.ReachedChaserPlatform[i, 0] && (i == 0 || safeZoneManager.ReachedChaserPlatform[i-1, 0]))
                {
                    var newChaser = LevelManager.DroneFactory.SpawnDrones(new DefaultDrone(_chaserBase, Vector3.zero, 0, playerManager));
                    safeZoneManager.Chasers.AddRange(newChaser);

                    if (_chaserSpawnPlatformIdxs[i] != 0) // sets start position of chaser
                    {
                        safeZoneManager.Chasers[i].transform.position = safeZones[_chaserStartPosition[i]].transform.position + safeZones[_chaserStartPosition[i]].transform.rotation *
                                                        new Vector3(safeZones[_chaserStartPosition[i]].transform.Find("VisibleObjects/Ground").transform.localScale.x / 2 + _chaserBase.Size / 2 + 0.5f, 0.4f, 0);
                    }
                    if (_pattern != null)
                    {
                        LevelManager.DroneFactory.AddPattern(_pattern, safeZoneManager.Chasers[i], _iDrone);
                    }
                    safeZoneManager.Chasers[i].tag = "Strong Enemy";
                    safeZoneManager.ReachedChaserPlatform[i, 0] = true;
                    safeZones[_chaserDestroyPlatformIdxs[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(true);
                    StartCoroutine(SpawnChaserText(3f));
                }
                if (_chaserDestroyPlatformIdxs[i] == platformIndex && !safeZoneManager.ReachedChaserPlatform[i, 1] && safeZoneManager.ReachedChaserPlatform[i, 0])
                {
                    Destroy(safeZoneManager.Chasers[i]);

                    safeZoneManager.ReachedChaserPlatform[i, 1] = true;
                    safeZones[_chaserDestroyPlatformIdxs[i]].transform.Find("PlayerCollider/ChaserGlow").gameObject.SetActive(false);
                    StartCoroutine(DestroyChaserText(3f));
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
