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



        public GameObject SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false, Area area = new Area())
        {
            GameObject newDrone = null;
            for (var i = 0; i < droneCount; i++)
            {
                newDrone = drone.CreateDroneInstance(this, isAdded, area);
                if (newDrone != null)
                    drone.ConfigureDrone(newDrone);
            }

            return newDrone;
        }

        public void AddDrones(IDrone drone, float delay, Area area)
        {
            StartCoroutine(DroneGenerator(drone, delay, area));
        }

        private IEnumerator DroneGenerator(IDrone drone, float delay, Area area)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SpawnDrones(drone, isAdded: true, area: area);
            }
        }

        public void SpawnAndAddDrones(IDrone drone, int droneCount, float delay, Area area = new Area())
        {
            SpawnDrones(drone, droneCount, area: area);
            AddDrones(drone, delay, area);
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
