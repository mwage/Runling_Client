using Launcher;
using UnityEngine;

namespace Drones.Movement
{
    public class ChaserMovement : ADroneMovement
    {
        public float Speed;

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
            if (GameControl.PlayerState.Player == null || _rb == null)
            {
                return;
            }
            if (!GameControl.PlayerState.Player.activeSelf || GameControl.PlayerState.IsImmobile)
            {
                _rb.velocity = Vector3.zero;
            }
            else
            {
                _targetPos = GameControl.PlayerState.Player.transform.position;
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

