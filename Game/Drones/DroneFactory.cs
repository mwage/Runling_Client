using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Pattern;
using Game.Scripts.Network;
using Game.Scripts.Network.Data;
using UnityEngine;

namespace Game.Scripts.Drones
{
    public class DroneFactory : MonoBehaviour
    {
        public DronePrefabs DronePrefabs;
        
        public Dictionary<ushort, DroneStateManager> Drones { get; } = new Dictionary<ushort, DroneStateManager>();
        public Dictionary<DroneType, GameObject> SetDroneType { get; } = new Dictionary<DroneType, GameObject>();
        public Dictionary<DroneColor, Material> SetDroneMaterial { get; } = new Dictionary<DroneColor, Material>();
        public bool IsServer { get; set; } = false;

        public delegate void SpawnDronesEventHandler(List<SpawnDroneData> droneDatas);
        public delegate void DestroyDroneEventHandler(ushort id);

        public event SpawnDronesEventHandler onSpawnDrones;
        public event DestroyDroneEventHandler onDestroyDrone;

        private ushort _lastId;

        private void Awake()
        {
            DronePrefabs.GetPrefabs(SetDroneType, SetDroneMaterial);
        }

        public List<GameObject> SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            var drones = new List<GameObject>();
            var droneDatas = new List<SpawnDroneData>();

            for (var i = 0; i < droneCount; i++)
            {
                var newDrone = drone.CreateDroneInstance(this, isAdded, area, posDelegate);
                newDrone.AddComponent<DroneManager>();

                if (IsServer)
                {
                    var data = AddDroneData(newDrone);
                    drone.ConfigureDrone(newDrone, this);
                    droneDatas.Add(new SpawnDroneData(
                        new DroneState(data.Id, newDrone.transform.position.x, newDrone.transform.position.z), 
                        drone.Speed, drone.Size, drone.Color, drone.DroneType));
                }
                else
                {
                    drone.ConfigureDrone(newDrone, this);
                }
                drones.Add(newDrone);
            }

            if (IsServer)
            {
                onSpawnDrones?.Invoke(droneDatas);
            }
            return drones;
        }

        public void SetPattern(IPattern pattern, IDrone drone, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            pattern.SetPattern(this, drone, area, posDelegate);
        }

        public void AddPattern( IPattern pattern, GameObject parentDrone, IDrone addedDrone, Area area = new Area())
        {
            pattern.AddPattern(this, parentDrone, addedDrone, area);
        }

        public void AddDrones(IDrone drone, float delay, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            StartCoroutine(GenerateDrones(drone, delay, area, posDelegate));
        }

        private IEnumerator GenerateDrones(IDrone drone, float delay, Area area, StartPositionDelegate posDelegate)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SpawnDrones(drone, isAdded: true, area: area, posDelegate: posDelegate);
            }
        }

        public void SpawnAndAddDrones(IDrone drone, int droneCount, float delay, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            SpawnDrones(drone, droneCount, area: area, posDelegate: posDelegate);
            AddDrones(drone, delay, area, posDelegate);
        }

        private DroneStateManager AddDroneData(GameObject drone)
        {
            do
            {
                _lastId++;
            } while (Drones.ContainsKey(_lastId));
            
            var droneData = drone.AddComponent<DroneStateManager>();
            droneData.Initialize(_lastId, this);
            Drones[_lastId] = droneData;
            return droneData;
        }

        public void DestroyDrone(ushort id)
        {
            onDestroyDrone?.Invoke(id);
        }
    }
}
