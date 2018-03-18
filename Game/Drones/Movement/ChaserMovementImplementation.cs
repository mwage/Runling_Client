using Game.Scripts.Players;
using UnityEngine;

namespace Game.Scripts.Drones.Movement
{
    public class ChaserMovementImplementation : ADroneMovementImplementation
    {
        private const float RotationSpeed = 15;
        private const float Acceleration = 50;

        private PlayerManager _target;
        private Vector3 _targetPos;
        private Vector3 _direction;
        private DroneManager _droneManager;
        private float _currentSpeed;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            _droneManager = GetComponent<DroneManager>();
        }

        public void Initialize(float speed, PlayerManager target)
        {
            Speed = speed;
            _target = target;
        }

        private void FixedUpdate()
        {
            if (_target == null)
                return;

            if (_target.IsDead)
            {
                Rb.velocity = Vector3.zero;
                return;
            }

            _targetPos = _target.transform.position;
            _targetPos.y += transform.position.y;
            _currentSpeed = _droneManager.IsFrozen ? 0F : Rb.velocity.magnitude;
                
            _direction = (_targetPos - transform.position).normalized;

            if ((_targetPos - transform.position).magnitude > 0.1f)
            {
                Rb.velocity = _direction * _currentSpeed;

                if (_currentSpeed < Speed)
                {
                    Rb.AddForce(_direction * Acceleration);
                }

                // Don't accelerate over maxSpeed
                else
                {
                    _currentSpeed = Speed;
                    Rb.velocity = _direction * _currentSpeed;
                }

                if (Mathf.Abs(_currentSpeed) > Mathf.Epsilon)
                {
                    Rotate();
                }
            }
            else
            {
                Rb.velocity = Vector3.zero;
            }
        }

        // Rotate Player
        private void Rotate()
        {
            var lookrotation = _targetPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), RotationSpeed * Time.deltaTime);
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

