using System.Security.Permissions;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level1RLR : ALevelRLR
    {
        public Level1RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            //DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(4f, 1f, Color.blue), 50, 1.5f, BoundariesRLR.BouncingRLR);
        }
    }
}
