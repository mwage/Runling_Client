using MP.TSGame.Drones.Types;
using UnityEngine;

namespace MP.TSGame.Drones.Pattern
{
    public interface IPattern
    {
        void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null);
        void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area);
    }
}
