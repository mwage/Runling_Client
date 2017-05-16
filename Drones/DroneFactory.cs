using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class DroneFactory : MonoBehaviour
    { 
        //attach gameobjects
        public GameObject BouncingDrone;
        public GameObject FlyingBouncingDrone;
        public GameObject FlyingOnewayDrone;
        public GameObject MineDrone;
        public GameObject MineDroneBouncing;
        public GameObject MineDroneOneway;

        public Material GreyMaterial;
        public Material BlueMaterial;
        public Material RedMaterial;
        public Material GoldenMaterial;
        public Material MagentaMaterial;
        public Material DarkGreenMaterial;
        public Material CyanMaterial;

        public Dictionary<DroneType,GameObject> SetDroneType = new Dictionary<DroneType,GameObject>();
        public Dictionary<DroneColor, Material> SetDroneMaterial = new Dictionary<DroneColor, Material>();

        private void Awake()
        {
            SetDroneType[DroneType.BouncingDrone] = BouncingDrone;
            SetDroneType[DroneType.FlyingBouncingDrone] = FlyingBouncingDrone;
            SetDroneType[DroneType.FlyingOnewayDrone] = FlyingOnewayDrone;
            SetDroneType[DroneType.MineDrone] = MineDrone;
            SetDroneType[DroneType.MineDroneBouncing] = MineDroneBouncing;
            SetDroneType[DroneType.MineDroneOneway] = MineDroneOneway;

            SetDroneMaterial[DroneColor.Grey] = GreyMaterial;
            SetDroneMaterial[DroneColor.Blue] = BlueMaterial;
            SetDroneMaterial[DroneColor.Red] = RedMaterial;
            SetDroneMaterial[DroneColor.Golden] = GoldenMaterial;
            SetDroneMaterial[DroneColor.Magenta] = MagentaMaterial;
            SetDroneMaterial[DroneColor.DarkGreen] = DarkGreenMaterial;
            SetDroneMaterial[DroneColor.Cyan] = CyanMaterial;
        }

        public List<GameObject> SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false, Area area = new Area(), StartPositionDelegate posDelegate = null)
        {
            var drones = new List<GameObject>();

            for (var i = 0; i < droneCount; i++)
            {
                var newDrone = drone.CreateDroneInstance(this, isAdded, area, posDelegate);
                if (newDrone != null)
                    drone.ConfigureDrone(newDrone, this);
                drones.Add(newDrone);
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
