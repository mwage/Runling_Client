using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Movement
{
    public class CurvedMovement : TrueSyncBehaviour
    {
        private TSRigidBody _rb;
        public FP Curving;
        public FP DroneSpeed;
        public FP? CurvingDuration;

        private void Awake()
        {
            _rb = GetComponent<TSRigidBody>();
        }

        public override void OnSyncedUpdate()
        {
            _rb.AddForce(_rb.tsTransform.right * -Curving, ForceMode.Acceleration);
            if (TSVector.Cross(_rb.velocity, TSVector.up) != TSVector.zero)
            {
                _rb.tsTransform.rotation = TSQuaternion.LookRotation(_rb.velocity, TSVector.up);
            }
            _rb.velocity = _rb.velocity.normalized * DroneSpeed;
            if (CurvingDuration != null && Curving > TrueSyncManager.DeltaTime * Curving / CurvingDuration.Value)
            {
                Curving -= TrueSyncManager.DeltaTime * Curving / CurvingDuration.Value;
            }
        }
    }
}
