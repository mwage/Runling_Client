using Drones.DroneTypes;

namespace SLA.Levels
{
    public class Level3SLA : ALevelSLA
    {
        public Level3SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1f, DroneColor.Magenta, DroneType.FlyingBouncingDrone), 120, 0.3f, BoundariesSLA.FlyingSla);
        }
    }
}
