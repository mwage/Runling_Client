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

        GridDrones GridDrones = new GridDrones();

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.15f, Color.blue), 7, 5f, BoundariesSLA.BouncingSla);

            // Spawn Grid Drones
            GridDrones.Grid(10, 0.05f, 7f, 1f, Color.magenta, BoundariesSLA.FlyingSla, DroneFactory, true);
        }        
    }
}
