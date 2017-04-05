using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level13SLA : ALevelSLA
    {
        public Level13SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 14;
        }

        public override void CreateDrones()
        {
            //Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(12f, 1f, Color.blue), 8, 6f);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(8f, 1.15f, Color.magenta), 8, 7f);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(4f, 1.5f, Color.red), 8, 8f);
        }
    }
}