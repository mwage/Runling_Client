using MP.TSGame.Drones.Types;

namespace MP.TSGame.SLA.Levels
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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6, 1.5f, DroneColor.Red), 11, 7, BoundariesSLA.BouncingSla);
            DroneFactory.BufferedSpawnAndAddDrones(new RandomDrone(6, 1, DroneColor.Magenta, DroneType.FlyingBouncingDrone), 70, 1, BoundariesSLA.FlyingSla, 1, 5);
        }
    }
}
