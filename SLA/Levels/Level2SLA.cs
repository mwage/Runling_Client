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
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1f, Color.blue), 25, 4f);
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1.5f, Color.red), 15, 8f);
        }
    }
}
