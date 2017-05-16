using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level2SLA : ALevelSLA
    {
        public Level2SLA(LevelManagerSLA manager) : base(manager)
        {
        }
        
        public override float GetMovementSpeed()
        {
            return 9;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1f, DroneColor.Blue), 25, 4f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1.5f, DroneColor.Red), 15, 8f, BoundariesSLA.BouncingSla);
        }
    }
}
