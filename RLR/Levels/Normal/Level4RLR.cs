using Drones.DroneTypes;

namespace RLR.Levels.Normal
{
    public class Level4RLR : ALevelRLR
    {
        public Level4RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(10, 1, DroneColor.Red, 3, LaneArea[0]), 80);
        }
    }
}
