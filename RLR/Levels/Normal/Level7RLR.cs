using Drones.DroneTypes;
using Drones.Pattern;

namespace RLR.Levels.Normal
{
    public class Level7RLR : ALevelRLR
    {
        public Level7RLR(LevelManagerRLR manager) : base(manager)
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

            // Spawn blue drones
            DroneFactory.SetPattern(new Pat360Drones(32, 10, true, true, 270), new DefaultDrone(8, 2, DroneColor.Blue));
        }
    }
}
