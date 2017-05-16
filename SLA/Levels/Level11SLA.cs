using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
{
    public class Level11SLA : ALevelSLA
    {
        public Level11SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7f, 1.5f, DroneColor.Red), 10, 7f, BoundariesSLA.BouncingSla);

            // Spawn Mine Drones
            MineVariations.Timed360Mines(DroneFactory, 3, new MineDrone(5, 3, DroneColor.Red), BoundariesSLA.FlyingSla,
                new DefaultDrone(10, 1, DroneColor.Cyan), 32, 8, 0.1f);
        }
    }
}