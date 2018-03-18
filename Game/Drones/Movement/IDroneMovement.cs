using UnityEngine;

namespace Game.Scripts.Drones.Movement
{
    public interface IDroneMovement
    {
        void Initialize(GameObject drone, float speed);
    }
}