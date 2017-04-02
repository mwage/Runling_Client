using UnityEngine;

namespace Assets.Scripts.Drones
{
    public interface IDrone
    {
        GameObject CreateDroneInstance(DroneFactory factory, bool isAdded);
        void ConfigureDrone(GameObject drone);
    }
}
