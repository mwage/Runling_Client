using Drones.DroneTypes;

namespace SLA.Levels
{
    public class Level5SLA : ALevelSLA
    {
        public Level5SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1.5f, DroneColor.Red), 12, 6f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1f, DroneColor.Magenta, DroneType.FlyingBouncingDrone), 80, 1f, BoundariesSLA.FlyingSla);
        }
    }
}
