using Assets.Scripts.Drones;
using Assets.Scripts.SLA.Levels;
using UnityEngine;

namespace Assets.Scripts.SLA.test
{
    public class DroneTestSLA : MonoBehaviour
    {
        // attach scripts
        public BoundariesSLA Area;
        public DroneFactory DroneFactory;

        public void LoadDrones()
        {
            // Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnDrones(new RandomBouncingDrone(5f, 1f, Color.grey), 5);
            DroneFactory.SpawnDrones(new RandomBouncingDrone(5f, 2f, Color.grey), 5);
        }
    }
}