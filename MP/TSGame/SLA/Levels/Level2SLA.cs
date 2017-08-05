using MP.TSGame.Drones.Types;

namespace MP.TSGame.SLA.Levels
{
    public class Level2SLA : ALevelSLA
    {
        public Level2SLA(LevelManagerSLA manager) : base(manager)
        {
        }
        
        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.BufferedSpawnAndAddDrones(new RandomDrone(5f, 1f, DroneColor.Blue), 20, 2.5f, BoundariesSLA.BouncingSla, 1, 2);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(5f, 1.5f, DroneColor.Red), 10, 8, BoundariesSLA.BouncingSla);
        }
    }
}
