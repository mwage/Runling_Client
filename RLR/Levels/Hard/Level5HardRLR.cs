using Drones.DroneTypes;

namespace RLR.Levels.Hard
{
    public class Level5HardRLR : ALevelRLR
    {
        public Level5HardRLR(LevelManagerRLR manager) : base(manager)
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

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(17, 2, DroneColor.Red, 3, LaneArea[0]), 70);
        }
    }
}
