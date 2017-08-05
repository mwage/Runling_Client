using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones.Movement
{
    public class SinusoidalMovement : TrueSyncBehaviour
    {
        private TSRigidBody _rb;
        private FP _initializationTime;
        private GameObject _droneModel;
        public FP SinFrequency;
        public FP SinForce;
        public FP DroneSpeed;
        public bool Fixed;


        private void Awake()
        {
            _initializationTime = TrueSyncManager.Time;
            _rb = GetComponent<TSRigidBody>();
            _droneModel = transform.Find("Model").gameObject;
        }

        public override void OnSyncedUpdate()
        {
            _rb.AddForce(_rb.tsTransform.right * SinForce * TSMath.Cos((TrueSyncManager.Time - _initializationTime) * SinFrequency), ForceMode.Acceleration);

            if (Fixed)
            {
                _rb.velocity = _rb.velocity.normalized * DroneSpeed;
            }

            _droneModel.GetComponent<TSTransform>().LookAt(_rb.position + _rb.velocity);

        }
    }
}
