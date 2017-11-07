using Drones.DroneTypes;

namespace RLR.Levels.Hard
{
    public class Level1HardRLR : ALevelRLR
    {
        public Level1HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9, 1f, DroneColor.BrightGreen), new[] { 1, 8, 16 }, new[] { 4, 12, 19 });
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(5, 1f, DroneColor.Grey), (int)(15 - i * 0.6f), area: LaneArea[i]);
            }

            // Middle reds
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[19], 5, true, DroneType.BouncingDrone), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[20], 5, true, DroneType.BouncingDrone), 4);
        }
    }
}
