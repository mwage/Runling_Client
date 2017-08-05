using System.Collections;
using Launcher;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Movement
{
    public class PointToPointMovement : TrueSyncBehaviour
    {
        public FP Acceleration;
        public FP MaxVelocity;
        public Area Area;
        public FP Size;
        public FP WaitTime;
        public bool Synchronized;

        private TSRigidBody _rb;
        private TSVector _startPosition, _movementVector;
        private bool _inWay = true;
        private bool _accPhase = true;
        private FP _movemementFraction, _movementLenght;
        private bool _inCoroutine;
        private FP _angle;
        private FP _startTime;
        private TSQuaternion _startRotation;
        private FP _endDelay;
        private FP _syncTime;


        private void Awake()
        {
            _rb = GetComponent<TSRigidBody>();
            SetLocationParameters();
            _endDelay = WaitTime / 4 > 1 ? 1 : WaitTime / 4;
            _syncTime = TrueSyncManager.Time;

        }

        public override void OnSyncedUpdate()
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
                            _rb.velocity = TSVector.zero;
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
                    TrueSyncManager.SyncedStartCoroutine(SetNewPosition());
                    _inCoroutine = true;
                }
                var t = (TrueSyncManager.Time - _startTime) / (WaitTime - _endDelay);
                _rb.tsTransform.rotation = TSQuaternion.Lerp(_startRotation,
                    TSQuaternion.Euler(0, _angle + _startRotation.eulerAngles.y, 0), t);
            }
        }

        private IEnumerator SetNewPosition()
        {
            SetLocationParameters();
            if (Synchronized && _startTime - _syncTime > WaitTime)
            {
                Debug.Log("Waittime too short, Synchronizationg failed, SyncTime: " + _syncTime);
                Synchronized = false;
            }
            yield return Synchronized ? WaitTime - (_startTime - _syncTime) : WaitTime;
            _syncTime = TrueSyncManager.Time;
            _inWay = true;
            _inCoroutine = false;
        }

        private void SetLocationParameters()
        {
            _rb.velocity = TSVector.zero;
            _movementVector = GetMovementVector();
            _movementLenght = _movementVector.magnitude;
            _startPosition = _rb.position;
            _angle = TSVector.Angle(_rb.tsTransform.forward, _movementVector);
            var cross = TSVector.Cross(_rb.tsTransform.forward, _movementVector).y;
            if (cross < 0) _angle = -_angle;
            _startTime = TrueSyncManager.Time;
            _startRotation = _rb.tsTransform.rotation;
        }

        private TSVector GetMovementVector()
        {
            return new TSVector(
                       GameControl.GameState.Random.Next(Area.LeftBoundary + 0.5f + (float)Size / 2, Area.RightBoundary - (0.5f + (float)Size / 2)), 0F,
                       GameControl.GameState.Random.Next(Area.BottomBoundary + 0.5f + (float)Size / 2, Area.TopBoundary - (0.5f + (float)Size / 2))) -
                   _rb.tsTransform.position;
        }

        public void OnSyncedCollisionEnter(TSCollision collision)
        {
            if (collision.gameObject.layer == 19)
            {
                _rb.velocity = TSVector.zero;
                _rb.tsTransform.position = _rb.tsTransform.position + collision.contacts[0].normal * 0.05f;
                _inWay = false;
                _accPhase = true;
            }
        }
    }
}
