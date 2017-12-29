using Drones.DroneTypes;
using Drones.Pattern;
using Network.Synchronization;
using Network.Synchronization.Data;
using Server.Scripts.Synchronization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drones
{
    public class DroneFactory : MonoBehaviour
    { 
        //attach gameobjects
        public GameObject BouncingDrone;
        public GameObject FlyingBouncingDrone;
        public GameObject FlyingOneWayDrone;
        public GameObject FlyingBouncingMine;
        public GameObject BouncingMine;
        public GameObject FlyingOneWayMine;

        public Material GreyMaterial;
        public Material BlueMaterial;
        public Material RedMaterial;
        public Material GoldenMaterial;
        public Material MagentaMaterial;
        public Material DarkGreenMaterial;
        public Material CyanMaterial;
        public Material BrightGreenMaterial;
        
        public Dictionary<DroneType, GameObject> SetDroneType { get; } = new Dictionary<DroneType,GameObject>();
        public Dictionary<DroneColor, Material> SetDroneMaterial { get; } = new Dictionary<DroneColor, Material>();

        public Dictionary<ushort, DroneStateManager> Drones { get; } = new Dictionary<ushort, DroneStateManager>();
        public bool IsServer { get; set; } = false;

        private ushort _lastId;

        private void Awake()
        {
            SetDroneType[DroneType.BouncingDrone] = BouncingDrone;
            SetDroneType[DroneType.FlyingBouncingDrone] = FlyingBouncingDrone;
            SetDroneType[DroneType.FlyingOneWayDrone] = FlyingOneWayDrone;
            SetDroneType[DroneType.FlyingBouncingMine] = FlyingBouncingMine;
            SetDroneType[DroneType.BouncingMine] = BouncingMine;
            SetDroneType[DroneType.FlyingOneWayMine] = FlyingOneWayMine;

            SetDroneMaterial[DroneColor.Grey] = GreyMaterial;
            SetDroneMaterial[DroneColor.Blue] = BlueMaterial;
            SetDroneMaterial[DroneColor.Red] = RedMaterial;
            SetDroneMaterial[DroneColor.Golden] = GoldenMaterial;
            SetDroneMaterial[DroneColor.Magenta] = MagentaMaterial;
            SetDroneMaterial[DroneColor.DarkGreen] = DarkGreenMaterial;
            SetDroneMaterial[DroneColor.Cyan] = CyanMaterial;
            SetDroneMaterial[DroneColor.BrightGreen] = BrightGreenMaterial;
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
                SyncDroneServer.SpawnDrones(droneDatas);
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
    }

    // Boundaries in which new drones can be spawned
    public struct Area
    {
        public float LeftBoundary;
        public float RightBoundary;
        public float TopBoundary;
        public float BottomBoundary;
    }
}
