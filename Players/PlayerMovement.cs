using System;
using System.Collections;
using Assets.Scripts.Launcher;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        public static float ZeroTolerance = 0.0001f;

        private Vector3 _clickPos;
        private Vector3 _targetPos;
        private Vector3 _direction;
        private float _rotationSpeed;
        private float _maxSpeed;
        private float _currentSpeed;
        private float _highestSpeedReached;
        private float _distance;
        private float _acceleration;
        private float _deceleration;
        private float _stopSensitivity;
        private bool _accelerate;
        private bool _stop;
        private bool _isAutoClicking;
        private int _defLayer;
        private Rigidbody _rb;
        private Coroutine _autoClickRoutine;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _targetPos = transform.position;
            _rotationSpeed = 15f;
            _acceleration = 150f;
            _deceleration = 100f;
            _currentSpeed = 0;
            _accelerate = false;
            _stop = true;
            _highestSpeedReached = 0;
            _stopSensitivity = 20;
            _defLayer = (1 << 0);
            _isAutoClicking = false;
        }

        private void Update()
        {
            if (GameControl.AutoClickerActive)
            {
                if (!_isAutoClicking)
                {
                    _autoClickRoutine = StartCoroutine(DoAutoclick());
                    _isAutoClicking = true;
                }
            }

            if (!GameControl.AutoClickerActive)
            {
                if (_isAutoClicking)
                {
                    StopCoroutine(_autoClickRoutine);
                    _isAutoClicking = false;
                }
            }


            // On right mouseclick, set new target location
            if (Input.GetMouseButtonDown(1))
            {
                MoveToPosition(Input.mousePosition);
            }
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
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _defLayer))
            {
                _maxSpeed = GameControl.MoveSpeed;
                _clickPos = hit.point;
                if ((_clickPos - transform.position).magnitude > 0.05f)
                {
                    _targetPos = _clickPos;
                    _direction = (_targetPos - transform.position).normalized;
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

        private void FixedUpdate()
        {
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
                    _rb.AddForce(_direction * _acceleration, ForceMode.Acceleration);
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
            if (collision.collider.tag == "Wall")
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
            var lookrotation = _targetPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }
    }
}