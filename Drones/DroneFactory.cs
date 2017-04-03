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

        public GameObject SpawnDrones(IDrone drone, int droneCount = 1, bool isAdded = false)
        {
            GameObject newDrone = null;
            for (var i = 0; i < droneCount; i++)
            {
                newDrone = drone.CreateDroneInstance(this, isAdded);
                if (newDrone != null)
                    drone.ConfigureDrone(newDrone);
            }

            return newDrone;
        }

        public void AddDrones(IDrone drone, float delay)
        {
            StartCoroutine(DroneGenerator(drone, delay));
        }

        private IEnumerator DroneGenerator(IDrone drone, float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SpawnDrones(drone, isAdded: true);
            }
        }

        public void SpawnAndAddDrones(IDrone drone, int droneCount, float delay)
        {
            SpawnDrones(drone, droneCount);
            AddDrones(drone, delay);
        }
    }
}
