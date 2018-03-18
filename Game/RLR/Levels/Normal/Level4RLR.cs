using Game.Scripts.Drones.DroneTypes;

namespace Game.Scripts.RLR.Levels.Normal
{
    public class Level4RLR : ALevelRLR
    {
        public Level4RLR(ILevelManagerRLR manager) : base(manager)
        {
        }

        public override void CreateDrones()
        {
            // Spawn red drones
            DroneFactory.SpawnDrones(new RedDrone(10, 1, DroneColor.Red, 3, LaneArea[0]), 80);
        }
    }
}
