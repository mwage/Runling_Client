using System;
using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        public static float ZeroTolerance = 0.001f;

        private Vector3 _clickPos;
        private Vector3 _targetPos;
        private Vector3 _direction;
        private float _targetRotation;
        private float _rotationSpeed;
        private float _maxSpeed;
        private float _currentSpeed;
        private float _highestSpeedReached;
        private float _distance;
        public float Acceleration = 50;
        private float _deceleration;
        private float _stopSensitivity;
        private bool _accelerate;
        private bool _stop;
        public bool IsAutoClicking;
        private int _defLayer;
        private Rigidbody _rb;
        private Coroutine _autoClickRoutine;
        private Animator _anim;
        private readonly int _speedHash = Animator.StringToHash("Speed");


        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _targetPos = transform.position;
            _rotationSpeed = 30;
            _deceleration = 100;
            _currentSpeed = 0;
            _accelerate = false;
            _stop = true;
            _highestSpeedReached = 0;
            _stopSensitivity = 20;
            _defLayer = (1 << 0);
            IsAutoClicking = false;
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (GameControl.AutoClickerActive)
            {
                if (!IsAutoClicking)
                {
                    _autoClickRoutine = StartCoroutine(DoAutoclick());
                    IsAutoClicking = true;
                }
            }

            if (!GameControl.AutoClickerActive)
            {
                if (IsAutoClicking)
                {
                    StopCoroutine(_autoClickRoutine);
                    IsAutoClicking = false;
                }
            }


            // On right mouseclick, set new target location
            if (Input.GetMouseButtonDown(1))
            {
                MoveToPosition(Input.mousePosition);
            }

            // Control animation
            _anim.SetFloat(_speedHash, _currentSpeed);
        }

        private IEnumerator DoAutoclick()
        {
            while (true)
            {
                MoveToPosition(Input.mousePosition);
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void MoveToPosition(Vector3 position)
        {
            if (!GameControl.IsImmobile)
            {
                RaycastHit hit;
                var ray = UnityEngine.Camera.main.ScreenPointToRay(position);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _defLayer))
                {
                    _maxSpeed = GameControl.MoveSpeed;
                    _clickPos = hit.point;
                    if ((_clickPos - transform.position).magnitude > 0.05f)
                    {
                        _targetPos = _clickPos;
                        _direction = (_targetPos - transform.position).normalized;
                        _targetRotation = Quaternion.LookRotation(_direction).eulerAngles.y;
                        _targetRotation = _targetRotation > 0 ? _targetRotation : 360 - _targetRotation;
                        _accelerate = true;
                        _stop = false;

                        if ((_targetPos - transform.position).magnitude < 0.5)
                        {
                            _rb.velocity = _direction * _currentSpeed / 2;
                        }
                        else
                        {
                            _rb.velocity = _direction * _currentSpeed;
                        }

                        _highestSpeedReached = _rb.velocity.magnitude;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (GameControl.IsImmobile)
            {
                _targetPos = _rb.transform.position;
            }
            _distance = (_targetPos - transform.position).magnitude;
            _currentSpeed = _rb.velocity.magnitude;

            // Stop
            if (_distance < _highestSpeedReached / (10 * _stopSensitivity) | _distance < 0.02f)
            {
                _rb.velocity = Vector3.zero;
                _stop = true;
                _accelerate = false;
                _currentSpeed = 0f;
            }

            // Accelerate
            if (_accelerate)
            {
                if (_currentSpeed < _maxSpeed)
                {
                    _rb.AddForce(_direction * Acceleration, ForceMode.Acceleration);
                    _currentSpeed = _rb.velocity.magnitude;
                    _highestSpeedReached = _currentSpeed;
                }

                // Don't accelerate over maxSpeed
                else
                {
                    _currentSpeed = _maxSpeed;
                    _rb.velocity = _direction * _currentSpeed;
                    _highestSpeedReached = _currentSpeed;
                }
            }

            // Decelerate
            if (_distance < _highestSpeedReached * _highestSpeedReached / (2 * _deceleration) && !_stop)
            {
                _rb.AddForce(_direction * (-_deceleration), ForceMode.Acceleration);
                _accelerate = false;
            }

            if (Math.Abs(_currentSpeed) > ZeroTolerance) { Rotate(); }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == 18)
            {
                transform.position = transform.position + collision.contacts[0].normal * 0.05f;
                if (Math.Abs(collision.contacts[0].normal.x) < ZeroTolerance)
                {
                    _rb.velocity = new Vector3(1, 0, 0) * _rb.velocity.x;
                    _targetPos = transform.position + new Vector3(0.1f, 0, 0) * _rb.velocity.x;
                    _direction = (_targetPos - transform.position).normalized;
                    _highestSpeedReached = _rb.velocity.magnitude;
                }
                else if (Math.Abs(collision.contacts[0].normal.z) < ZeroTolerance)
                {
                    _rb.velocity = new Vector3(0, 0, 1) * _rb.velocity.z;
                    _targetPos = transform.position + new Vector3(0.1f, 0, 0) * _rb.velocity.z;
                    _direction = (_targetPos - transform.position).normalized;
                    _highestSpeedReached = _rb.velocity.magnitude;
                }
                else
                {
                    _rb.velocity = Vector3.zero;
                    _stop = true;
                    _accelerate = false;
                }
            }
        }

        // Rotate Player
        void Rotate()
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

            if (Mathf.Abs(_targetRotation - lookRotation) > _rotationSpeed * difference / 90)
            {
                _rb.transform.Rotate(0, sign * _rotationSpeed * difference / 180, 0);
            }
        }
    }
}