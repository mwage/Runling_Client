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
            return 8;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(4f, 1f, DroneColor.Blue), 50, 1.5f, BoundariesSLA.BouncingSla);
        }
    }
}
