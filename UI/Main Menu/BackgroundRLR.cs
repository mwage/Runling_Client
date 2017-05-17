using Drones;
using Drones.DroneTypes;
using Drones.Movement;
using Drones.Pattern;
using RLR.GenerateMap;
using UnityEngine;

namespace UI.Main_Menu
{
    public class BackgroundRLR : MonoBehaviour
    {
        public GenerateMapRLR GenerateMap;
        public DroneFactory DroneFactory;

        private void Start()
        {
            GenerateMap.GenerateMap(15, new float[] { 8, 6, 8, 6, 8 }, 1.2f, 0.3f, 10);

            var laneArea = GenerateMap.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(7, 2f, DroneColor.Grey), (int)(12 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(7, 2f, DroneColor.Grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(7, 2f, DroneColor.Grey), 3, area: laneArea[20]);

            // Spawn blue drones
            DroneFactory.SetPattern(new Pat360Drones(48, 10, true, true, 270), new DefaultDrone(15, 2, DroneColor.Blue));
            DroneFactory.SetPattern(new Pat360Drones(48, 10, true, true, 90), new DefaultDrone(15, 2, DroneColor.Blue));

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, repeat: true, pulseDelay: 6),
                new DefaultDrone(15, 2, DroneColor.Golden, moveDelegate: DroneMovement.CurvedMovement, curving: 7));
        }
    }
}
