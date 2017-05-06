using System.Collections;
using Assets.Scripts.Drones;
using UnityEngine;

namespace Assets.Scripts.SLA.Levels
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
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6f, 1f, Color.blue), 10, 4f, BoundariesSLA.BouncingSla);
            DroneFactory.SpawnAndAddDrones(new RandomDrone(6f, 1.5f, Color.red), 8, 8f, BoundariesSLA.BouncingSla);

            // Spawn Green Drones
            DroneFactory.SetPattern(new Pat360Drones(numRays: 12, delay: 2.5f, repeat: true, clockwise: true,
                    startRotation: -90, maxRotation: 180,
                    pulseDelay: 5, reducePulseDelay: 0.05f, minPulseDelay: 2, changeDirection: true,
                    addRays: 1, maxRays: 32, limitedRepeats: 2, reduceDelay: 0.03f, minDelay: 1.5f),
                new DefaultDrone(7, 1.5f, Color.green), posDelegate: delegate {
                    return new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.BottomBoundary + (0.5f + 1.5f / 2)); });

            DroneFactory.SetPattern(new Pat360Drones(numRays: 12, delay: 2.5f, repeat: true, clockwise: false,
                    startRotation: -90, maxRotation: 180,
                    pulseDelay: 5, reducePulseDelay: 0.05f, minPulseDelay: 2, initialDelay: 5, changeDirection: true,
                    addRays: 1, maxRays: 32, limitedRepeats: 2, reduceDelay: 0.03f, minDelay: 1.5f),
                new DefaultDrone(7, 1.5f, Color.green), posDelegate: delegate {
                    return new Vector3(0, 0.6f, BoundariesSLA.FlyingSla.TopBoundary - (0.5f + 1.5f / 2)); });
        }
    }
}