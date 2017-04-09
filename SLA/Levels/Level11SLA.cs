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
            DroneFactory.SpawnAndAddDrones(new RandomBouncingDrone(7f, 1.5f, Color.red), 10, 7f, BoundariesSLA.BouncingSla);

            // Spawn Mine Drones
            var mines = new GameObject[3];
            for (var i = 0; i < mines.Length; i++)
            {
                mines[i] = DroneFactory.SpawnDrones(new MineDrone(5f, 3f, Color.black), area: BoundariesSLA.BouncingSla);
            }

            MineVariations.AddStraightFlying360Drones(32, 10f, 8f, 1f, Color.cyan, mines, DroneFactory, 0.1f);
        }
    }
}