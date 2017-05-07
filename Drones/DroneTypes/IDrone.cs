using UnityEngine;

namespace Assets.Scripts.Drones
{
    public interface IDrone
    {
        GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);
        void ConfigureDrone(GameObject drone);
        float GetSize();
        float GetSpeed();
        Color GetColor();
        DroneType GetDroneType();
    }
}
