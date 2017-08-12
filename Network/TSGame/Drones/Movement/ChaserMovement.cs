using MP.TSGame.Players;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Movement
{
    public class ChaserMovement : TrueSyncBehaviour
    {
        public PlayerManager Target;
        public FP Speed;

        private readonly int _rotationSpeed = 15;
        private readonly FP _acceleration = 50;

        private TSVector _targetPos;
        private TSVector _direction;
        private TSRigidBody _rb;
        private FP _maxSpeed;

        private void Awake()
        {
            _rb = GetComponent<TSRigidBody>();
            _maxSpeed = Speed;
        }

        public override void OnSyncedUpdate()
        {
            Debug.Log("here");
            if (Target == null || _rb == null)
                return;


            if (Target.IsDead || Target.IsImmobile)
            {
                _rb.velocity = TSVector.zero;
            }
            else
            {
                _targetPos = Target.tsTransform.position;
                _targetPos.y = 0.4f;
                var currentSpeed = _rb.velocity.magnitude;
                _direction = (_targetPos - _rb.tsTransform.position).normalized;

                if ((_targetPos - _rb.tsTransform.position).magnitude > 0.1f)
                {
                    _rb.velocity = _direction * currentSpeed;

                    if (currentSpeed < _maxSpeed)
                    {
                        _rb.AddForce(_direction * _acceleration);
                    }

                    // Don't accelerate over maxSpeed
                    else
                    {
                        currentSpeed = _maxSpeed;
                        _rb.velocity = _direction * currentSpeed;
                    }

                    if (TSMath.Abs(currentSpeed) > 0.001)
                    {
                        Rotate();
                    }
                }
                else
                {
                    _rb.velocity = TSVector.zero;
                }
            }
        }

        // Rotate Player
        private void Rotate()
        {
            var lookrotation = _targetPos - _rb.tsTransform.position;
            _rb.tsTransform.rotation = TSQuaternion.Slerp(_rb.tsTransform.rotation, TSQuaternion.LookRotation(lookrotation), _rotationSpeed * TrueSyncManager.DeltaTime);
        }
    }
}
