using System;
using Launcher;
using UnityEngine;

namespace Players
{
    public class MovementTest : MonoBehaviour
    {
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
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _targetPos = transform.position;
            _rotationSpeed = 15f;
            _acceleration = 100f;
            _deceleration = 100f;
            _currentSpeed = 0;
            GameControl.State.MoveSpeed = 10;
            _accelerate = false;
            _stop = true;
            _highestSpeedReached = 0;
            _stopSensitivity = 20;
        }

        private void Update()
        {
            // On right mouseclick, set new target location
            if (!Input.GetMouseButtonDown(1)) return;
            RaycastHit hit;
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit)) return;

            _maxSpeed = GameControl.State.MoveSpeed;
            _clickPos = hit.point;
            _clickPos.y = 0;
            if (!((_clickPos - transform.position).magnitude > 0.05f)) return;

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
                    _accelerate = false;
                    _highestSpeedReached = _currentSpeed;
                }
            }

            // Decelerate
            if (_distance < _highestSpeedReached * _highestSpeedReached / (2 * _deceleration) && !_stop)
            {
                _rb.AddForce(_direction * (-_deceleration), ForceMode.Acceleration);
                _accelerate = false;
            }

            if (Math.Abs(_currentSpeed) > PlayerMovement.ZeroTolerance) { Rotate(); }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider.CompareTag("Wall"))
            {
                transform.position = transform.position + collision.contacts[0].normal * 0.05f;
                if (Math.Abs(collision.contacts[0].normal.x) < PlayerMovement.ZeroTolerance)
                {
                    _rb.velocity = new Vector3(1, 0, 0) * _rb.velocity.x;
                    _targetPos = transform.position + new Vector3(0.1f, 0, 0) * _rb.velocity.x;
                    _direction = (_targetPos - transform.position).normalized;
                    _highestSpeedReached = _rb.velocity.magnitude;
                }
                else if (Math.Abs(collision.contacts[0].normal.z) < PlayerMovement.ZeroTolerance)
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
        private void Rotate()
        {
            var lookrotation = _targetPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }
    }
}