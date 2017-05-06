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

        public static Dictionary<DroneType,GameObject> SetDroneType = new Dictionary<DroneType,GameObject>();

        private void Awake()
        {
            SetDroneType[DroneType.BouncingDrone] = BouncingDrone;
            SetDroneType[DroneType.FlyingBouncingDrone] = FlyingBouncingDrone;
            SetDroneType[DroneType.FlyingOnewayDrone] = FlyingOnewayDrone;
            SetDroneType[DroneType.MineDrone] = MineDrone;
        }

        public List<GameObject> SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false, Area area = new Area(), StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null, IPattern pattern = null)
        {
            var drones = new List<GameObject>();

            if (pattern == null)
            {
                
                for (var i = 0; i < droneCount; i++)
                {
                    var newDrone = drone.CreateDroneInstance(this, isAdded, area, posDelegate);
                    if (newDrone != null)
                        drone.ConfigureDrone(newDrone, moveDelegate);
                    drones.Add(newDrone);
                }
            }
            else
            {
                pattern.SetPattern(this, drone, area, posDelegate, moveDelegate);
            }
            return drones;
        }

        public void SetPattern(IPattern pattern, IDrone drone, Area area = new Area(), StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            pattern.SetPattern(this, drone, area, posDelegate, moveDelegate);
        }

        public void AddPattern( IPattern pattern, GameObject parentDrone, IDrone addedDrone, Area area = new Area(), DroneMovement.MovementDelegate moveDelegate = null)
        {
            pattern.AddPattern(this, parentDrone, addedDrone, area, moveDelegate);
        }

        public void AddDrones(IDrone drone, float delay, Area area = new Area(), StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            StartCoroutine(GenerateDrones(drone, delay, area, posDelegate, moveDelegate));
        }

        private IEnumerator GenerateDrones(IDrone drone, float delay, Area area, StartPositionDelegate posDelegate, DroneMovement.MovementDelegate moveDelegate)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SpawnDrones(drone, isAdded: true, area: area, posDelegate: posDelegate);
            }
        }

        public void SpawnAndAddDrones(IDrone drone, int droneCount, float delay, Area area = new Area(), StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            SpawnDrones(drone, droneCount, area: area, posDelegate: posDelegate, moveDelegate: moveDelegate);
            AddDrones(drone, delay, area, posDelegate, moveDelegate);
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

    public enum DroneType
    {
        BouncingDrone,
        FlyingBouncingDrone,
        FlyingOnewayDrone,
        MineDrone
    }
}
