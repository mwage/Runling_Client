using UnityEngine;

namespace Drones.Movement
{
    public abstract class ADroneMovement : MonoBehaviour
    {
        protected Rigidbody Rb;
        protected bool IsSlowed;
        protected float SlowPercentage;
        protected bool IsFrozen;
        protected Vector3 VelocityBeforeFreezeNotSlowed;

        public abstract void Move();
        public abstract void Freeze();
        public abstract void UnFreeze();
        public abstract void SlowDown(float percentage);
        public abstract void UnSlowDown();
    }
}