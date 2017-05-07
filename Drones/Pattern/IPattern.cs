using UnityEngine;

namespace Assets.Scripts.Drones
{
    public interface IPattern
    {
        void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null);
        void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area);
    }
}
