using MP.TSGame.Drones.Movement;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Types
{
    public class DefaultDrone : ADrone
    {
        protected readonly TSVector Position;
        protected readonly FP Direction;

        public DefaultDrone(IDrone sourceDrone, TSVector position, FP direction)
        {
            CopyFrom(sourceDrone);
            Position = position;
            Direction = direction;
        }

        public DefaultDrone(float speed, float size, DroneColor color, TSVector? position = null, FP? direction = null, DroneType? droneType = null, 
            DroneMovement.MovementDelegate moveDelegate = null, FP? curving = null, FP? sinForce = null, FP? sinFrequency = null, GameObject player = null) : 
            base(speed, size, color, droneType, moveDelegate, curving, sinForce, sinFrequency, player)
        {
            Position = position ?? new TSVector(0, 0.4f, 0);
            Direction = direction ?? 0;
            DroneType = droneType ?? DroneType.FlyingOneWayDrone;
        }

        public override GameObject CreateDroneInstance(DroneFactory factory, bool isAdded, Area area, StartPositionDelegate posDelegate = null)
        {
            var newDrone = TrueSyncManager.SyncedInstantiate(factory.SetDroneType[DroneType], Position,
                TSQuaternion.Euler(0, Direction, 0));

            newDrone.transform.SetParent(factory.transform);
            return newDrone;
        }
    }
}
