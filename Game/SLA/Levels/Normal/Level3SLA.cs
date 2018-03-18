using Game.Scripts.Drones.DroneTypes;

namespace Game.Scripts.SLA.Levels.Normal
{
    public class Level3SLA : ALevelSLA
    {
        public Level3SLA(ILevelManagerSLA manager) : base(manager)
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
