using Drones.DroneTypes;

namespace SLA.Levels
{
    public class Level5SLA : ALevelSLA
    {
        public Level5SLA(ILevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6, 1.5f, DroneColor.Red), 11, 7, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6, 1f, DroneColor.Magenta, DroneType.FlyingBouncingDrone), 70, 1f, BoundariesSLA.FlyingSla);
        }
    }
}
