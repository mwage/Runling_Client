using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;

namespace RLR.Levels.Hard
{
    public class Level9HardRLR : ALevelRLR
    {
        public Level9HardRLR(LevelManagerRLR manager) : base(manager)
        {
        }

        protected override float SetAirCollider()
        {
            return 10;
        }

        public override void SetChasers()
        {
            Manager.RunlingChaser.SetChaserPlatforms(new DefaultDrone(9, 1f, DroneColor.BrightGreen), new[] { 1}, new[] { 4});
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), (int)(11 - i * 0.4f), area: LaneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), 4, area: LaneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), 3, area: LaneArea[20]);

            // Spawn blue drones
            DroneFactory.SetPattern(new Pat360Drones(40, 9, true, true, 270), new DefaultDrone(12, 2, DroneColor.Blue));
            DroneFactory.SetPattern(new Pat360Drones(40, 9, true, true, 90), new DefaultDrone(12, 2, DroneColor.Blue));

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, repeat: true, pulseDelay: 5),
                new DefaultDrone(12, 2, DroneColor.Golden, movementType: new CurvedMovement(10)));
        }
    }
}
