using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Pattern;

namespace Game.Scripts.RLR.Levels.Hard
{
    public class Level2HardRLR : ALevelRLR
    {
        public Level2HardRLR(ILevelManagerRLR manager) : base(manager)
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
                DroneFactory.SpawnDrones(new RandomDrone(5, 1, DroneColor.Grey), (int)(11 - i * 0.4f), area: LaneArea[i]);
            }

            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.15f, 2),
                new RandomDrone(10, 1, DroneColor.Blue, restrictedZone: 0, droneType: DroneType.FlyingOneWayDrone));

            // Middle reds
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[19], 5, true, DroneType.BouncingDrone), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[20], 5, true, DroneType.BouncingDrone), 4);
        }
    }
}
