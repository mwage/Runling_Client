using Assets.Scripts.Drones;
using Assets.Scripts.SLA.Levels;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class BackgroundSLA : MonoBehaviour
    {
        public DroneFactory DroneFactory;

        void Start()
        {
            DroneFactory.SpawnDrones(new RandomDrone(3f, 1f, Color.blue), 20, area: BoundariesSLA.BouncingMainMenu);
            DroneFactory.SpawnDrones(new RandomDrone(5f, 1f, Color.magenta, DroneType.FlyingBouncingDrone), 30, area: BoundariesSLA.FlyingMainMenu);
        }
    }
}

