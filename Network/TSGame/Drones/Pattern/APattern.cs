using MP.TSGame.Drones.Types;
using UnityEngine;

namespace MP.TSGame.Drones.Pattern
{
    public abstract class APattern : IPattern
    {
        public abstract void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null);

        public virtual void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area)
        {
            Debug.Log("AddPattern not implemented for this Pattern");
        }
    }
}
