using Drones.DroneTypes;
using Drones.Pattern;

namespace SLA.Levels
{
    public class Level7SLA : ALevelSLA
    {
        public Level7SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1.5f, DroneColor.Red), 15, 5, BoundariesSLA.BouncingSla);

            DroneFactory.SpawnDrones(new MineDrone(5, 3, DroneColor.Red, new Pat360Drones(56, 4, true, false, 0, 720, changeDirection: true),
                new DefaultDrone(8, 1.3f, DroneColor.Cyan)), area: BoundariesSLA.FlyingSla);
        }
    }
}