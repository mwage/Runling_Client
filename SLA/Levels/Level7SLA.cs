using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level7SLA : ALevelSLA
    {
        public Level7SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6f, 1.5f, Color.red), 15, 6f, BoundariesSLA.BouncingSla);

            DroneFactory.SpawnDrones(new MineDrone(5, 3, Color.black, new Pat360Drones(32, 2, true, false, 0, 360, 2),
                new OnewayDrone(8, 1.3f, Color.cyan)), area: BoundariesSLA.FlyingSla);
        }
    }
}