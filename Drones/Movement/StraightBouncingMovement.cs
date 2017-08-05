using UnityEngine;

namespace Drones.Movement
{
    public class StraightBouncingMovement : ADroneMovement
    {
        private void Start()
        {
            //_droneManager = GetComponent<DroneManager>();
            _rb = gameObject.GetComponent<Rigidbody>();
        }

        public override void Move()
        {

        }

        public override void Freeze()
        {
            _isFrozen = true;
            if (_isSlowed)
            {
                _velocityBeforeFreezeNotSlowed = _rb.velocity / (1 - _slowPercentage);
            }
            else
            {
                _velocityBeforeFreezeNotSlowed = _rb.velocity;
            }
            _rb.velocity = Vector3.zero;
        }

        public override void UnFreeze()
        {
            _isFrozen = false;
            if (!_isSlowed)
            {
                _rb.velocity = _velocityBeforeFreezeNotSlowed;
            }
            else
            {
                _rb.velocity = _velocityBeforeFreezeNotSlowed;
                _rb.velocity *= (1 - _slowPercentage);
            }
        }

        public override void SlowDown(float percentage)
        {
            _isSlowed = true;
            _slowPercentage = percentage;
            if (_isFrozen)
            {
                // dont change speed
            }
            else
            {
                _rb.velocity *= (1 - percentage);
            }
        }

        public override void UnSlowDown()
        {
            _isSlowed = false;
            
            if (!_isFrozen)
            {
                _rb.velocity *= 1 / (1 - _slowPercentage);
            }
            else
            {
                // dont change speed
            }
            _slowPercentage = 0F;

        }
    }
    

}
