using System;
using UnityEngine;

namespace Game.Scripts.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables

        public const int DefLayer = 1 << 15; // Ground Layer for Raycasting;

        private const float RotationSpeed = 40;
        private const float Acceleration = 150;
        private const float Deceleration = 100;
        private const float MinRegisterDistance = 0.15f;
        private const float StopSensitivity = 20; // Adjust for better accuracy at other decelerations

        private PlayerManager _playerManager;
        private Rigidbody _rb;

        private Vector3 _targetPos, _currentPos;
        private Vector3 _normal;
        private Vector3 _direction, _rotatedDirection;
        private float _targetRotation;
        private float _maxSpeed, _currentSpeed, _highestSpeedReached;
        private float _distance, _lastDistance;
        private int _distanceCounter;

        private bool _accelerate;
        private bool _stop = true;

        private Animator _anim;
        private readonly int _speedHash = Animator.StringToHash("Speed");


        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _rb = GetComponent<Rigidbody>();
            _anim = _playerManager.GetComponent<Animator>();
            _targetPos = transform.position;
        }
        #endregion

        private void Update()
        {
            // Control animation
            _anim.SetFloat(_speedHash, _currentSpeed);
        }

        public void MoveToPosition(Vector3 clickPos)
        {
            if (_playerManager.IsImmobile || (clickPos - transform.position).magnitude < MinRegisterDistance)
                    return;

            _targetPos = new Vector3(clickPos.x, 0, clickPos.z);
            _direction = (_targetPos - _currentPos).normalized;
            GetRelativeDirection();

            _targetRotation = Quaternion.LookRotation(_direction).eulerAngles.y;
            _targetRotation = _targetRotation > 0 ? _targetRotation : 360 - _targetRotation;
            _accelerate = true;
            _stop = false;

            if ((_targetPos - transform.position).magnitude < 0.5f)
            {
                _rb.velocity = _rotatedDirection * _currentSpeed / 2;
            }
            else
            {
                _rb.velocity = _rotatedDirection * _currentSpeed;
            }

            _highestSpeedReached = _rb.velocity.magnitude;
            _lastDistance = Mathf.Infinity;
            _distanceCounter = 3;
        }

        #region Physics

        private void FixedUpdate()
        {
            _maxSpeed = _playerManager.CharacterManager.Speed.Current;
            _currentPos = new Vector3(_rb.transform.position.x, 0, _rb.transform.position.z);
            _currentSpeed = _rb.velocity.magnitude;

            GetRelativeDirection();

            if (_playerManager.IsImmobile)
            {
                _targetPos = _currentPos;
            }

            _distance = (_targetPos - _currentPos).magnitude;

            // Stop - Accelerate - Decelerate Conditions
            if (_distance < _highestSpeedReached / (10 * StopSensitivity) || _distance < 0.02f || _lastDistance < _distance && _distanceCounter < 1)
            {
                Stop();
            }
            if (_accelerate && _distance > _currentSpeed * _currentSpeed / (2 * Deceleration))
            {
                Accelerate();
            }
            if (_distance < _highestSpeedReached * _highestSpeedReached / (2 * Deceleration) && !_stop || _currentSpeed > _maxSpeed + 0.2f)
            {
                Decelerate();
            }

            if (Math.Abs(_currentSpeed) > Mathf.Epsilon) { Rotate(); }

            _lastDistance = _distance;
            if (_distanceCounter > 0)
            {
                _distanceCounter--;
            }
        }

        // Stop Player
        private void Stop()
        {
            _rb.velocity = Vector3.zero;
            _stop = true;
            _accelerate = false;
            _currentSpeed = 0f;
        }

        // Accelerate Player
        private void Accelerate()
        {
            if (_currentSpeed < _maxSpeed)
            {
                _rb.AddForce(_rotatedDirection * Acceleration, ForceMode.Acceleration);
                _currentSpeed = _rb.velocity.magnitude;
                _highestSpeedReached = _currentSpeed;
            }
            else
            {
                _currentSpeed = _rb.velocity.magnitude;
            }
        }

        // Decelerate Player
        private void Decelerate()
        {
            _rb.AddForce(-_rotatedDirection * Deceleration, ForceMode.Acceleration);
        }

        private void GetRelativeDirection()
        {
            RaycastHit hit;

            if (Physics.Raycast(_rb.transform.position + _rb.rotation * new Vector3(0, 0.1f, 0), Vector3.down, out hit, Mathf.Infinity, DefLayer))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    _normal = hit.normal;
                    Physics.gravity = -_normal * 100;
                    var angle = new Vector3(
                        (90 - Vector3.Angle(_normal, Vector3.right)) * Mathf.Sin(_rb.rotation.eulerAngles.y * Mathf.PI / 180) +
                        (90 - Vector3.Angle(_normal, Vector3.forward)) * Mathf.Sin((_rb.rotation.eulerAngles.y + 90) * Mathf.PI / 180),
                        _rb.rotation.eulerAngles.y,
                        (90 - Vector3.Angle(_normal, Vector3.right)) * Mathf.Sin((_rb.rotation.eulerAngles.y - 90) * Mathf.PI / 180) +
                        (90 - Vector3.Angle(_normal, Vector3.forward)) * Mathf.Sin(_rb.rotation.eulerAngles.y * Mathf.PI / 180));

                    _rb.transform.rotation = Quaternion.Euler(angle);

                    _rotatedDirection = new Vector3(_direction.x,
                        -Mathf.Sin(((90 - Vector3.Angle(_normal, Vector3.right)) * Mathf.Sin(_targetRotation * Mathf.PI / 180) +
                                    (90 - Vector3.Angle(_normal, Vector3.forward)) * Mathf.Sin((_targetRotation + 90) * Mathf.PI / 180)) * Mathf.PI / 180),
                        _direction.z);
                }
            }
        }

        #endregion

        // Wall Collision
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == 10)
            {
                transform.position = transform.position + collision.contacts[0].normal * 0.05f;
                _lastDistance = Mathf.Infinity;
                _distanceCounter = 3;

                if (Math.Abs(collision.contacts[0].normal.x) < Mathf.Epsilon)
                {
                    _rb.velocity = new Vector3(1, 0, 0) * _rb.velocity.x;
                    _targetPos = _rb.transform.position + new Vector3(0.1f, 0, 0) * _rb.velocity.x;
                    _direction = (_targetPos - transform.position).normalized;
                    _highestSpeedReached = _rb.velocity.magnitude;
                }
                else if (Math.Abs(collision.contacts[0].normal.z) < Mathf.Epsilon)
                {
                    _rb.velocity = new Vector3(0, 0, 1) * _rb.velocity.z;
                    _targetPos = transform.position + new Vector3(0, 0, 0.1f) * _rb.velocity.z;
                    _direction = (_targetPos - transform.position).normalized;
                    _highestSpeedReached = _rb.velocity.magnitude;
                }
                else
                {
                    _rb.velocity = Vector3.zero;
                    _targetPos = transform.position;
                    _stop = true;
                    _accelerate = false;
                }
            }
        }

        // Rotate Player
        private void Rotate()
        {
            var lookRotation = _rb.transform.rotation.eulerAngles.y;
            int sign;
            float difference;
            lookRotation = lookRotation > 0 ? lookRotation : 360 - lookRotation;

            if (_targetRotation > lookRotation)
            {
                difference = Mathf.Abs(_targetRotation - lookRotation) < 360 - Mathf.Abs(_targetRotation - lookRotation)
                    ? Mathf.Abs(_targetRotation - lookRotation) : 360 - Mathf.Abs(_targetRotation - lookRotation);
                sign = Mathf.Abs(_targetRotation - lookRotation) < 360 - Mathf.Abs(_targetRotation - lookRotation) ? 1 : -1;
            }
            else
            {
                difference = Mathf.Abs(lookRotation - _targetRotation) < 360 - Mathf.Abs(lookRotation - _targetRotation)
                    ? Mathf.Abs(lookRotation - _targetRotation) : 360 - Mathf.Abs(lookRotation - _targetRotation);
                sign = Mathf.Abs(lookRotation - _targetRotation) < 360 - Mathf.Abs(lookRotation - _targetRotation) ? -1 : 1;
            }

            if (Mathf.Abs(_targetRotation - lookRotation) > RotationSpeed * difference / 90)
            {
                _rb.transform.Rotate(0, sign * RotationSpeed * difference / 180, 0);
            }
        }
    }
}