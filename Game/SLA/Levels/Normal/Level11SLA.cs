using Game.Scripts.Drones;
using Game.Scripts.Drones.DroneTypes;

namespace Game.Scripts.SLA.Levels.Normal
{
    public class Level11SLA : ALevelSLA
    {
        public Level11SLA(ILevelManagerSLA manager) : base(manager)
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

            // Spawn Mine Drones
            MineVariations.Synced360Mines(DroneFactory, 3, new MineDrone(5, 3, DroneColor.Red), BoundariesSLA.FlyingSla,
                new DefaultDrone(10, 1, DroneColor.Cyan), 32, 8, 0.03f);
        }
    }
}