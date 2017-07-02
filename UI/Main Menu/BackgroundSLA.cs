using Drones;
using Drones.DroneTypes;
using SLA.Levels;
using UnityEngine;

namespace UI.Main_Menu
{
    public class BackgroundSLA : MonoBehaviour
    {
        public DroneFactory DroneFactory;

        private void Start()
        {
            DroneFactory.SpawnDrones(new RandomDrone(3f, 1f, DroneColor.Blue), 20, area: BoundariesSLA.BouncingMainMenu);
            DroneFactory.SpawnDrones(new RandomDrone(5f, 1f, DroneColor.Magenta, DroneType.FlyingBouncingDrone), 50, area: BoundariesSLA.FlyingMainMenu);
        }
    }
}

