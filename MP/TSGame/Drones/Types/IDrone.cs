using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Types
{
    public interface IDrone
    {
        FP Size { get; }
        GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);
        void ConfigureDrone(GameObject drone, DroneFactory factory);
    }
}
