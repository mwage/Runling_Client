using System.Collections;
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

        public GameObject SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false, Area area = new Area(), StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            GameObject newDrone = null;
            for (var i = 0; i < droneCount; i++)
            {
                newDrone = drone.CreateDroneInstance(this, isAdded, area, posDelegate);
                if (newDrone != null)
                    drone.ConfigureDrone(newDrone, moveDelegate);
            }

            return newDrone;
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

        public void SpawnAndAddDrones(IDrone drone, int droneCount, float delay, Area area = new Area(), StartPositionDelegate posDelegate = null, DroneMovement.MovementDelegate moveDelegate = null)
        {
            SpawnDrones(drone, droneCount, area: area, posDelegate: posDelegate, moveDelegate: moveDelegate);
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
