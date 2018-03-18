using Game.Scripts.Drones;
using Game.Scripts.Drones.DroneTypes;
using UnityEngine;

namespace Game.Scripts.SLA
{
    public interface ILevelManagerSLA
    {
        DroneFactory DroneFactory { get; }
        void LoadDrones(int level);
        void SpawnChaser(IDrone drone, Vector3? position = null);
        float GetMovementSpeed(int level);
        void EndLevel(float delay);
    }
}