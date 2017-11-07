using System;
using System.Collections;
using UnityEngine;

namespace Players
{
    public class MovementTest : MonoBehaviour
    {
        #region Variables

        public GameObject MouseClickPrefab;

        private const float RotationSpeed = 30;
        private const float Acceleration = 100;
        private const float Deceleration = 100;
        private const float StopSensitivity = 20; // Adjust for better accuracy at other decelerations
        private const float MaxSpeed = 10f;

        public bool AutoClickerActive;
        public bool IsAutoClicking;

        private PlayerManager _playerManager;
        private Rigidbody _rb;

        private Vector3 _targetPos, _clickPos, _currentPos;
        private Vector3 _normal;
        private Vector3 _direction, _rotatedDirection;
        private float _targetRotation;
        private float _currentSpeed, _highestSpeedReached;
        private float _distance, _lastDistance;
        private int _distanceCounter;

        private bool _accelerate;
        private bool _stop = true;

        private Coroutine _autoClickRoutine;
        private const int DefLayer = 1 << 15; // Ground Layer for Raycasting;
        private Animator _anim;
        private readonly int _speedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _rb = GetComponent<Rigidbody>();
            _targetPos = transform.position;
            _anim = GetComponent<Animator>();
        }
        #endregion

        #region UserInput
        private void Update()
        {
            if (AutoClickerActive)
            {
                if (!IsAutoClicking)
                {
                    _autoClickRoutine = StartCoroutine(DoAutoclick());
                    IsAutoClicking = true;
                }
            }


            if (!AutoClickerActive)
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
            RaycastHit hit;
            var ray = UnityEngine.Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, DefLayer))
            {
                _clickPos = hit.point;

                // Play click animation
                var click = Instantiate(MouseClickPrefab, _clickPos, Quaternion.Euler(0, 45, 0));
                if (_playerManager.IsImmobile)
                {
                    foreach (Transform child in click.transform)
                    {
                        child.GetComponent<Renderer>().material.color = Color.red;
                        foreach (Transform ch in child)
                        {
                            ch.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                }
                else
                {
                    if ((_clickPos - transform.position).magnitude < 0.05f) return;

                    _targetPos = new Vector3(_clickPos.x, 0, _clickPos.z);
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
            }
        }

        #endregion

        #region Physics
        private void FixedUpdate()
        {
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
            if (_distance < _highestSpeedReached * _highestSpeedReached / (2 * Deceleration) && !_stop || _currentSpeed > MaxSpeed + 0.2f)
            {
                Decelerate();
            }

            if (Math.Abs(_currentSpeed) > Mathf.Epsilon) { Rotate(); }


            //Debug.Log(Physics.gravity);

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
            if (_currentSpeed < MaxSpeed)
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