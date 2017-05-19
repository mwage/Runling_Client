using Drones.DroneTypes;

namespace SLA.Levels
{
    public class Level1SLA : ALevelSLA
    {
        public Level1SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(4, 1, DroneColor.Blue), 60, 1, BoundariesSLA.BouncingSla);
        }
    }
}
