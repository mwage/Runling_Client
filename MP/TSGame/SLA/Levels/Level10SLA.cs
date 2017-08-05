using MP.TSGame.Drones.Pattern;
using MP.TSGame.Drones.Types;

namespace MP.TSGame.SLA.Levels
{
    public class Level10SLA : ALevelSLA
    {
        public Level10SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(7, 1.15f, DroneColor.Blue), 10, 4, BoundariesSLA.BouncingSla);

            // Spawn Grid Drones
            DroneFactory.SetPattern(new PatGridDrones(10, 0.05f, true), new DefaultDrone(7f, 1f, DroneColor.Magenta),
                BoundariesSLA.FlyingSla);
        }        
    }
}
