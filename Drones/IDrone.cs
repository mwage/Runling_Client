using UnityEngine;

namespace Assets.Scripts.Drones
{
    public interface IDrone
    {
        GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area);
        void ConfigureDrone(GameObject drone);
    }
}
