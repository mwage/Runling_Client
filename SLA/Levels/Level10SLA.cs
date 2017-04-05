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
            //Spawn drones (dronecount/delay, speed, size, color)
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1f, Color.blue), 10, 4f);

            // Grid Drones
            GridDrones.Grid(10, 0.05f, 7f, 1f, Color.magenta, BoundariesSLA.FlyingSla, DroneFactory, true);
        }        
    }
}
