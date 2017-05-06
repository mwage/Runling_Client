using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level3SLA : ALevelSLA
    {
        public Level3SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 9;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1f, Color.magenta, DroneType.FlyingBouncingDrone), 130, 0.3f, BoundariesSLA.FlyingSla);
        }
    }
}
