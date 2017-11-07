using UnityEngine;

namespace Drones.Movement
{
    public abstract class ADroneMovementImplementation : MonoBehaviour
    {
        protected Rigidbody Rb { get; set; }
        protected float Speed { get; set; }
        protected bool IsSlowed { get; set; }
        protected float SlowPercentage { get; set; }
        protected bool IsFrozen { get; set; }
        protected Vector3 VelocityBeforeFreezeNotSlowed { get; set; }

        public abstract void Move();
        public abstract void Freeze();
        public abstract void UnFreeze();
        public abstract void SlowDown(float percentage);
        public abstract void UnSlowDown();
    }
}