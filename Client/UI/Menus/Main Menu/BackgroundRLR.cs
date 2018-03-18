using Game.Scripts.Drones;
using Game.Scripts.Drones.DroneTypes;
using Game.Scripts.Drones.Movement;
using Game.Scripts.Drones.Pattern;
using Game.Scripts.RLR.MapGenerator;
using UnityEngine;

namespace Client.Scripts.UI.Menus.Main_Menu
{
    public class BackgroundRLR : MonoBehaviour
    {
        public MapGeneratorRLR MapGenerator;
        public DroneFactory DroneFactory;

        private void Start()
        {
            MapGenerator.GenerateMap(15, new float[] { 8, 6, 8, 6, 8 }, 1.2f, 0.3f, 10);

            var laneArea = MapGenerator.GetDroneSpawnArea();

            // Spawn bouncing drones
            for (var i = 1; i < laneArea.Length - 2; i++)
            {
                DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), (int)(11 - i * 0.4f), area: laneArea[i]);
            }
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), 4, area: laneArea[19]);
            DroneFactory.SpawnDrones(new RandomDrone(6, 2f, DroneColor.Grey), 3, area: laneArea[20]);

            // Spawn blue drones
            DroneFactory.SetPattern(new Pat360Drones(40, 9, true, true, 270), new DefaultDrone(12, 2, DroneColor.Blue));
            DroneFactory.SetPattern(new Pat360Drones(40, 9, true, true, 90), new DefaultDrone(12, 2, DroneColor.Blue));

            // Spawn yellow drones
            DroneFactory.SetPattern(new Pat360Drones(32, repeat: true, pulseDelay: 5),
                new DefaultDrone(12, 2, DroneColor.Golden, movementType: new CurvedMovement(10)));
        }
    }
}
