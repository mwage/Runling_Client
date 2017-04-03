using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level5SLA : ALevel
    {
        public Level5SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 9;
        }

        public override void CreateDrones()
        {
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(5f, 1.5f, Color.red), 15, 6f);
            DroneFactory.SpawnAndAddDrones(new RandomFlyingBouncingDrone(5f, 1f, Color.magenta), 100, 1f);
        }
    }
}
