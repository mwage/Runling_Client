using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Drones
{
    public class PointToPointMovement : MonoBehaviour
    {
        public float Acceleration;
        public float MaxVelocity;
        public Area Area;
        public float Size;
        public float WaitTime;
        public bool Synchronized;

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

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            SetLocationParameters();
            _endDelay = WaitTime / 4 > 1 ? 1 : WaitTime / 4;
            _syncTime = Time.time;

        }

        private void FixedUpdate()
        {
            if (_inWay)
            {
                if (_accPhase)
                {
                    if (_rb.velocity.magnitude < MaxVelocity &&
                        (_rb.position - _startPosition).magnitude / _movementLenght < 0.5)
                    {
                        _rb.AddForce(_movementVector.normalized * Acceleration, ForceMode.Acceleration);
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
                            _rb.AddForce(_movementVector.normalized * (-Acceleration), ForceMode.Acceleration);
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
                var t = (Time.time - _startTime) / (WaitTime - _endDelay);
                _rb.transform.rotation = Quaternion.Lerp(_startRotation,
                    Quaternion.Euler(0, _angle + _startRotation.eulerAngles.y, 0), t);
            }
        }

        IEnumerator SetNewPosition()
        {
            SetLocationParameters();
            if (Synchronized && _startTime - _syncTime > WaitTime)
            {
                Debug.Log("Waittime too short, Synchronizationg failed, SyncTime: " + _syncTime);
                Synchronized = false;
            }
            yield return new WaitForSeconds(Synchronized ? WaitTime - (_startTime - _syncTime) : WaitTime);
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
            if (cross < 0) _angle = -_angle;
            _startTime = Time.time;
            _startRotation = _rb.transform.rotation;
        }

        private Vector3 GetMovementVector()
        {
            return new Vector3(
                       Random.Range(Area.LeftBoundary + 0.5f + Size / 2, Area.RightBoundary - (0.5f + Size / 2)), 0F,
                       Random.Range(Area.BottomBoundary + 0.5f + Size / 2, Area.TopBoundary - (0.5f + Size / 2))) -
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
    }
}
