using UnityEngine;

namespace Drones.Movement
{
    public interface IDroneMovement
    {
        void Initialize(GameObject drone, float speed);
    }
}