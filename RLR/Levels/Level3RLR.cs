using Assets.Scripts.Drones;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.RLR.Levels
{
    public class Level3RLR : ALevelRLR
    {
        public Level3RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 2),
                new RandomDrone(6, 1, Color.blue, restrictedZone: 0, droneType: DroneType.FlyingOnewayDrone), moveDelegate: DroneMovement.CurvedMovement);
        }
    }
}