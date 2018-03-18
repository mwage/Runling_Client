using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Movement;
using Game.Scripts.Drones.Pattern;

namespace Game.Scripts.RLR.Levels.Hard
{
    public class Level7HardRLR : ALevelRLR
    {
        public Level7HardRLR(ILevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2, DroneColor.Grey), (int)(11 - i * 0.4f), area: LaneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2, DroneColor.Grey), 4, area: LaneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2, DroneColor.Grey), 3, area: LaneArea[20]);

            // Spawn yellow drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 1),
                new RandomDrone(14, 2, DroneColor.Golden, DroneType.FlyingOneWayDrone, 0, movementType: new SinusoidalMovement(40, 5f)));
        }
    }
}
