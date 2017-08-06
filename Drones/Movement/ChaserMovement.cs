using Players;
using UnityEngine;

namespace Drones.Movement
{
    public class ChaserMovement : ADroneMovement
    {
        public float Speed;
        public PlayerManager ChaserTarget;

        private float _rotationSpeed;
        private Vector3 _targetPos;
        private Vector3 _direction;
        private Rigidbody _rb;
        private float _acceleration;
        private float _maxSpeed;
        private DroneManager _droneManager;
        private float _currentSpeed;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _droneManager = gameObject.GetComponent<DroneManager>();
            _rotationSpeed = 15f;
            _acceleration = 50f;
            _maxSpeed = Speed;
        }

        private void FixedUpdate()
        {
            if (ChaserTarget == null || _rb == null)
            {
                return;
            }
            if (ChaserTarget.IsDead)
            {
                _rb.velocity = Vector3.zero;
            }
            else
            {
                _targetPos = ChaserTarget.transform.position;
                _targetPos.y += 0.4f;
                _currentSpeed = _droneManager.IsFrozen ? 0F : _rb.velocity.magnitude;
                
                _direction = (_targetPos - transform.position).normalized;

                if ((_targetPos - transform.position).magnitude > 0.1f)
                {
                    _rb.velocity = _direction * _currentSpeed;

                    if (_currentSpeed < _maxSpeed)
                    {
                        _rb.AddForce(_direction * _acceleration);
                    }

                    // Don't accelerate over maxSpeed
                    else
                    {
                        _currentSpeed = _maxSpeed;
                        _rb.velocity = _direction * _currentSpeed;
                    }

                    if (Mathf.Abs(_currentSpeed) > 0.00001)
                    {
                        Rotate();
                    }
                }
                else
                {
                    _rb.velocity = Vector3.zero;
                }
            }
        }

        // Rotate Player
        private void Rotate()
        {
            var lookrotation = _targetPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }

        public override void Move()
        {

        }

        public override void Freeze()
        {
            IsFrozen = true;
            if (IsSlowed)
            {
                VelocityBeforeFreezeNotSlowed = _rb.velocity / (1 - SlowPercentage);
            }
            else
            {
                VelocityBeforeFreezeNotSlowed = _rb.velocity;
            }
            _rb.velocity = Vector3.zero;
        }

        public override void UnFreeze()
        {
            IsFrozen = false;
            if (!IsSlowed)
            {
                _rb.velocity = VelocityBeforeFreezeNotSlowed;
            }
            else
            {
                _rb.velocity = VelocityBeforeFreezeNotSlowed;
                _rb.velocity *= (1 - SlowPercentage);
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
                _rb.velocity *= (1 - percentage);
            }
        }

        public override void UnSlowDown()
        {
            IsSlowed = false;

            if (!IsFrozen)
            {
                _rb.velocity *= 1 / (1 - SlowPercentage);
            }
            else
            {
                // dont change speed
            }
            SlowPercentage = 0F;

        }
    }
}

