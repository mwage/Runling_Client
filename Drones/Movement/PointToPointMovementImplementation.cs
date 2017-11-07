using System.Collections;
using UnityEngine;

namespace Drones.Movement
{
    public class PointToPointMovementImplementation : ADroneMovementImplementation
    {
        private float _maxSpeed;
        private Area _area;
        private float _size;
        private float _acceleration;
        private float _waitTime;
        private bool _synchronized;

        private Rigidbody _rb;
        private Vector3 _startPosition, _movementVector;
        private bool _inWay = true;
        private bool _accPhase = true;
        private float _movemementFraction, _movementLenght;
        private bool _inCoroutine;
        private float _angle;
        private float _startTime;
        private Quaternion _startRotation;
        private float _endDelay;
        private float _syncTime;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Initialize(float maxSpeed, Area area, float size, float acceleration, float waitTime, bool synchronized)
        {
            _acceleration = acceleration;
            _maxSpeed = maxSpeed;
            _area = area;
            _size = size;
            _waitTime = waitTime;
            _synchronized = synchronized;

            SetLocationParameters();
            _endDelay = _waitTime / 4 > 1 ? 1 : _waitTime / 4;
            _syncTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (_inWay)
            {
                if (_accPhase)
                {
                    if (_rb.velocity.magnitude < _maxSpeed &&
                        (_rb.position - _startPosition).magnitude / _movementLenght < 0.5)
                    {
                        _rb.AddForce(_movementVector.normalized * _acceleration, ForceMode.Acceleration);
                    }
                    else
                    {
                        _accPhase = false;
                        _movemementFraction = (_rb.position - _startPosition).magnitude / _movementLenght;
                    }
                }
                else
                {
                    if ((_rb.position - _startPosition).magnitude / _movementLenght > 1 - _movemementFraction)
                    {
                        if (_rb.velocity.magnitude > 0.1)
                        {
                            _rb.AddForce(_movementVector.normalized * (-_acceleration), ForceMode.Acceleration);
                        }
                        else
                        {
                            _rb.velocity = Vector3.zero;
                            _inWay = false;
                            _accPhase = true;
                        }
                    }
                }

            }
            else
            {
                if (!_inCoroutine)
                {
                    StartCoroutine(SetNewPosition());
                    _inCoroutine = true;
                }
                var t = (Time.time - _startTime) / (_waitTime - _endDelay);
                _rb.transform.rotation = Quaternion.Lerp(_startRotation,
                    Quaternion.Euler(0, _angle + _startRotation.eulerAngles.y, 0), t);
            }
        }

        private IEnumerator SetNewPosition()
        {
            SetLocationParameters();
            if (_synchronized && _startTime - _syncTime > _waitTime)
            {
                Debug.Log("Waittime too short, Synchronizationg failed, SyncTime: " + _syncTime);
                _synchronized = false;
            }
            yield return new WaitForSeconds(_synchronized ? _waitTime - (_startTime - _syncTime) : _waitTime);
            _syncTime = Time.time;
            _inWay = true;
            _inCoroutine = false;
        }

        private void SetLocationParameters()
        {
            _rb.velocity = Vector3.zero;
            _movementVector = GetMovementVector();
            _movementLenght = _movementVector.magnitude;
            _startPosition = _rb.position;
            _angle = Vector3.Angle(_rb.transform.forward, _movementVector);

            var cross = Vector3.Cross(_rb.transform.forward, _movementVector).y;
            if (cross < 0)
            {
                _angle = -_angle;
            }
            _startTime = Time.time;
            _startRotation = _rb.transform.rotation;
        }

        private Vector3 GetMovementVector()
        {
            return new Vector3(
                       Random.Range(_area.LeftBoundary + 0.5f + _size / 2, _area.RightBoundary - (0.5f + _size / 2)), 0F,
                       Random.Range(_area.BottomBoundary + 0.5f +_size / 2, _area.TopBoundary - (0.5f + _size / 2))) -
                   _rb.transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 19)
            {
                _rb.velocity = Vector3.zero;
                _rb.transform.position = _rb.transform.position + collision.contacts[0].normal * 0.05f;
                _inWay = false;
                _accPhase = true;
            }
        }

        public override void Move()
        {

        }

        public override void Freeze()
        {
            throw new System.NotImplementedException();
        }

        public override void UnFreeze()
        {
            throw new System.NotImplementedException();
        }

        public override void SlowDown(float percentage)
        {
            throw new System.NotImplementedException();
        }

        public override void UnSlowDown()
        {
            throw new System.NotImplementedException();
        }
    }
}
