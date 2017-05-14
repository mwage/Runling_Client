using UnityEngine;

namespace Assets.Scripts.Drones
{
    public abstract class APattern : IPattern
    {
        protected float Speed;
        protected float Size;
        protected Color Color;
        protected DroneType DroneType;
        protected DroneMovement.MovementDelegate MoveDelegate;
        protected GameObject Player;
        protected float? Curving;
        protected float? SinForce;
        protected float? SinFrequency;

        protected APattern()
        {
        }

        public abstract void SetPattern(DroneFactory factory, IDrone drone, Area area, StartPositionDelegate posDelegate = null);

        public virtual void AddPattern(DroneFactory factory, GameObject drone, IDrone addedDrone, Area area)
        {
            Debug.Log("AddPattern not implemented for this Pattern");
        }

        protected void SetParameters(IDrone drone)
        {
            Speed = (float)drone.GetParameters()[0];
            Size = (float)drone.GetParameters()[1];
            Color = (Color)drone.GetParameters()[2];
            DroneType = (DroneType)drone.GetParameters()[3];
            MoveDelegate = (DroneMovement.MovementDelegate)drone.GetParameters()[4];
            Player = (GameObject)drone.GetParameters()[5];
            Curving = (float?)drone.GetParameters()[6];
            SinForce = (float?)drone.GetParameters()[7];
            SinFrequency = (float?)drone.GetParameters()[8];
        }
    }
}
