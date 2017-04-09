using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level10SLA : ALevelSLA
    {
        public Level10SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.15f, Color.blue), 7, 5f, BoundariesSLA.BouncingSla);

            // Spawn Grid Drones
            DroneFactory.SpawnDrones(new GridDrones(7f, 1f, Color.magenta, 10, 0.05f, true), area: BoundariesSLA.FlyingSla);
        }        
    }
}
