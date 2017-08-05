using UnityEngine;

namespace Drones.Movement
{
    public class CurvedMovement : ADroneMovement
    {
        private Rigidbody _rb;
        public float Curving;
        public float DroneSpeed;
        public float? CurvingDuration;

        private void Start ()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * -Curving, ForceMode.Acceleration);
            if (Vector3.Cross(_rb.velocity, Vector3.up) != Vector3.zero)
            {
                _rb.transform.rotation = Quaternion.LookRotation(_rb.velocity, Vector3.up);
            }
            _rb.velocity = _rb.velocity.normalized * DroneSpeed;
            if (CurvingDuration != null && Curving > Time.fixedDeltaTime * Curving / CurvingDuration.Value)
            {
                Curving -= Time.fixedDeltaTime * Curving / CurvingDuration.Value;
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
