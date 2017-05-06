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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7f, 1.15f, Color.blue), 7, 5f, BoundariesSLA.BouncingSla);

            // Spawn Grid Drones
            DroneFactory.SetPattern(new PatGridDrones(10, 0.05f), new DefaultDrone(7f, 1f, Color.magenta),
                BoundariesSLA.FlyingSla);
        }        
    }
}
