using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;

namespace RLR.Levels.Normal
{
    public class Level3RLR : ALevelRLR
    {
        public Level3RLR(LevelManagerRLR manager) : base(manager)
        {
        }

        protected override float SetAirCollider()
        {
            return 10;
        }

        public override void CreateDrones()
        {
            // Spawn blue drones
            DroneFactory.SetPattern(new PatContinuousSpawn(0.1f, 1),
                new RandomDrone(6, 1, DroneColor.Blue, restrictedZone: 0, droneType: DroneType.FlyingOneWayDrone, moveDelegate: DroneMovement.CurvedMovement));
        }
    }
}