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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(11f, 1f, DroneColor.Blue), 8, 6f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7f, 1.15f, DroneColor.Magenta), 8, 7f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(4f, 1.5f, DroneColor.Red), 8, 8f, BoundariesSLA.BouncingSla);
        }
    }
}