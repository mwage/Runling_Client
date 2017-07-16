using System.Collections;
using Drones;
using Drones.DroneTypes;
using Drones.Pattern;
using SLA.Levels;
using UnityEngine;

namespace UI.Main_Menu
{
    public class BackgroundSLA : MonoBehaviour
    {
        public DroneFactory DroneFactory;

        private void Start()
        {
            // Spawn Bouncing Drones
            DroneFactory.SpawnDrones(new RandomDrone(7, 1f, DroneColor.Blue), 15, area: BoundariesSLA.BouncingMainMenu);
            DroneFactory.SpawnDrones(new RandomDrone(7, 1.5f, DroneColor.Red), 10, area: BoundariesSLA.BouncingMainMenu);

            // Spawn Green Drones
            DroneFactory.StartCoroutine(GenerateGreenDrones(2.5f, 24, 7, 1.5f, DroneColor.DarkGreen));

        }

        private IEnumerator GenerateGreenDrones(float delay, int initialDroneCount, float speed, float size,
            DroneColor color)
        {
            var droneCount = 0;

            while (true)
            {
                DroneFactory.SetPattern(
                    new Pat360Drones(initialDroneCount + droneCount, delay, false, true, -90, 180,
                        changeDirection: true, patternRepeats: 2),
                    new DefaultDrone(speed, size, color), posDelegate: delegate
                    {
                        return new Vector3(0, 0.4f, BoundariesSLA.FlyingMainMenu.BottomBoundary + (0.5f + 1.5f / 2));
                    });
                yield return new WaitForSeconds(2 * delay);
                DroneFactory.SetPattern(
                    new Pat360Drones(initialDroneCount + droneCount, delay, false, false, -90, 180,
                        changeDirection: true, patternRepeats: 2),
                    new DefaultDrone(speed, size, color), posDelegate: delegate
                    {
                        return new Vector3(0, 0.4f, BoundariesSLA.FlyingMainMenu.TopBoundary - (0.5f + 1.5f / 2));
                    });

                yield return new WaitForSeconds(2 * delay);
            }
        }
    }
}

