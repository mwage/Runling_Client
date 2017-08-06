using UnityEngine;

namespace Drones.Movement
{
    public class StraightBouncingMovement : ADroneMovement
    {
        private void Start()
        {
            //_droneManager = GetComponent<DroneManager>();
            Rb = gameObject.GetComponent<Rigidbody>();
        }

        public override void Move()
        {

        }

        public override void Freeze()
        {
            IsFrozen = true;
            if (IsSlowed)
            {
                VelocityBeforeFreezeNotSlowed = Rb.velocity / (1 - SlowPercentage);
            }
            else
            {
                VelocityBeforeFreezeNotSlowed = Rb.velocity;
            }
            Rb.velocity = Vector3.zero;
        }

        public override void UnFreeze()
        {
            IsFrozen = false;
            if (!IsSlowed)
            {
                Rb.velocity = VelocityBeforeFreezeNotSlowed;
            }
            else
            {
                Rb.velocity = VelocityBeforeFreezeNotSlowed;
                Rb.velocity *= (1 - SlowPercentage);
            }
        }

        public override void SlowDown(float percentage)
        {
            IsSlowed = true;
            SlowPercentage = percentage;
            if (IsFrozen)
            {
                // dont change speed
            }
            else
            {
                Rb.velocity *= (1 - percentage);
            }
        }

        public override void UnSlowDown()
        {
            IsSlowed = false;
            
            if (!IsFrozen)
            {
                Rb.velocity *= 1 / (1 - SlowPercentage);
            }
            else
            {
                // dont change speed
            }
            SlowPercentage = 0F;

        }
    }
    

}
