using Game.Scripts.Drones.DroneTypes;

namespace Game.Scripts.RLR.Levels.Normal
{
    public class Level5RLR : ALevelRLR
    {
        public Level5RLR(ILevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn bouncing drones
            for (var i = 1; i < LaneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(4.5f, 1f, DroneColor.Grey), (int) (10 - i * 0.35f), area: LaneArea[i]);
                DroneFactory.SpawnDrones(new RandomDrone(4.5f, 2f, DroneColor.Grey), (int) (5 - i * 0.15f), area: LaneArea[i]);
            }
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[19], 5, true, DroneType.BouncingDrone), 5);
            DroneFactory.SpawnDrones(new RedDrone(500, 1, DroneColor.Red, 1000, LaneArea[20], 5, true, DroneType.BouncingDrone), 4);
        }
    }
}
