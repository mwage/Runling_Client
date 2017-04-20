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
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(11f, 1f, Color.blue), 8, 6f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.15f, Color.magenta), 8, 7f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(4f, 1.5f, Color.red), 8, 8f, BoundariesSLA.BouncingSla);
        }
    }
}