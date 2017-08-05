using UnityEngine;

namespace Drones.Movement
{
    public abstract class ADroneMovement : MonoBehaviour
    {
        protected Rigidbody _rb;
        protected bool _isSlowed;
        protected float _slowPercentage;
        protected bool _isFrozen;
        protected Vector3 _velocityBeforeFreezeNotSlowed;

        public abstract void Move();
        public abstract void Freeze();
        public abstract void UnFreeze();
        public abstract void SlowDown(float percentage);
        public abstract void UnSlowDown();
    }
}