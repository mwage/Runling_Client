using Drones.DroneTypes;

namespace RLR.Levels.Normal
{
    public class Level8RLR : ALevelRLR
    {
        public Level8RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), (int)(12 - i * 0.4f), area: LaneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), 4, area: LaneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(5, 2f, DroneColor.Grey), 3, area: LaneArea[20]);

            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(13, 2, DroneColor.Red, 3, LaneArea[0]), 60);
        }
    }
}
