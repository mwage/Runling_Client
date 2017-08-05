using MP.TSGame.Drones.Types;

namespace MP.TSGame.SLA.Levels
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
            DroneFactory.BufferedSpawnAndAddDrones(new RandomDrone(5, 1, DroneColor.Blue), 40, 1.5f, BoundariesSLA.BouncingSla, 1.5, 3);
        }
    }
}
