using Drones.Movement;
using Network.Synchronization;
using UnityEngine;

namespace Drones.DroneTypes
{
    public interface IDrone
    {
        float Size { get; }
        float Speed { get; }
        DroneColor Color { get; }
        DroneType DroneType { get; }
        IDroneMovement MovementType { get; set; }
        GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null);
        void ConfigureDrone(GameObject drone, DroneFactory factory, DroneStateManager data = null);
    }
}
