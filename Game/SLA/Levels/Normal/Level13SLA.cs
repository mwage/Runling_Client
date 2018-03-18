using Game.Scripts.Drones.DroneTypes;

namespace Game.Scripts.SLA.Levels.Normal
{
    public class Level13SLA : ALevelSLA
    {
        public Level13SLA(ILevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 14;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(13, 1f, DroneColor.Blue), 7, 6, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1.15f, DroneColor.Magenta), 7, 7, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(4, 1.5f, DroneColor.Red), 7, 9, BoundariesSLA.BouncingSla);
        }
    }
}