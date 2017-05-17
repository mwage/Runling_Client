using Drones.DroneTypes;

namespace RLR.Levels.Normal
{
    public class Level1RLR : ALevelRLR
    {
        public Level1RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length-2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(2f, 1f, DroneColor.Grey), (int)(13-i*0.5f), area: LaneArea[i]);
            }


            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[19], 5, true, DroneType.MineDroneBouncing), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[20], 5, true, DroneType.MineDroneBouncing), 4);
        }
    }
}
