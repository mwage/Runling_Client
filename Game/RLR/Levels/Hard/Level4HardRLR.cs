using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Pattern;

namespace Game.Scripts.RLR.Levels.Hard
{
    public class Level4HardRLR : ALevelRLR
    {
        public Level4HardRLR(ILevelManagerRLR manager) : base(manager)
        {
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

            // Spawn yblue drones
            DroneFactory.SetPattern(new Pat360Drones(32, repeat: true, pulseDelay: 5),
                new DefaultDrone(12, 2, DroneColor.Blue));
        }
    }
}
