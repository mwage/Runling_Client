using Drones.DroneTypes;

namespace SLA.Levels
{
    public class Level2SLA : ALevelSLA
    {
        public Level2SLA(ILevelManagerSLA manager) : base(manager)
        {
        }
        
        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1f, DroneColor.Blue), 20, 2.5f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1.5f, DroneColor.Red), 10, 8, BoundariesSLA.BouncingSla);
        }
    }
}
