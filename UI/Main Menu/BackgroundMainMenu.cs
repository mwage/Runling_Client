using Assets.Scripts.Drones;
using Assets.Scripts.SLA.Levels;
using UnityEngine;

namespace Assets.Scripts.UI.Main_Menu
{
    public class BackgroundMainMenu : MonoBehaviour
    {
        //attach scripts
        public BoundariesSLA Area;
        public DroneFactory DroneFactory;


        void Start()
        {
            DroneFactory.SpawnDrones(new RandomBouncingDrone(3f, 1f, Color.blue), 20, area: BoundariesSLA.BouncingSla);
            DroneFactory.SpawnDrones(new RandomFlyingBouncingDrone(5f, 1f, Color.magenta), 30, area: BoundariesSLA.FlyingSla);
        }
    }
}

