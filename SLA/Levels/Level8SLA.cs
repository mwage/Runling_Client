using Drones.DroneTypes;
using Drones.Pattern;
using UnityEngine;

namespace SLA.Levels
{
    public class Level8SLA : ALevelSLA
    {
        public Level8SLA(LevelManagerSLA manager) : base(manager)
        {
        }

        public override float GetMovementSpeed()
        {
            return 11;
        }

        public override void CreateDrones()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6f, 1f, DroneColor.Blue), 10, 4f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6f, 1.5f, DroneColor.Red), 8, 8f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.SetPattern(new Pat360Drones(12, 2.5f, true, true, -90, 180, 5, 0.05f, 2, null, true, 1, 32, 2, 0.03f, 1.5f),
                new DefaultDrone(7, 1.5f, DroneColor.DarkGreen), posDelegate: delegate {
                    return new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.BottomBoundary + (0.5f + 1.5f / 2)); });

            DroneFactory.SetPattern(new Pat360Drones(12, 2.5f, true, false, -90, 180, 5, 0.05f, 2, 5, true, 1, 32, 2, 0.03f, 1.5f),
                new DefaultDrone(7, 1.5f, DroneColor.DarkGreen), posDelegate: delegate {
                    return new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.TopBoundary - (0.5f + 1.5f / 2)); });
        }
    }
}