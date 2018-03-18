using UnityEngine;

namespace Game.Scripts.Drones.Movement
{
    public class CurvedMovementImplementation : ADroneMovementImplementation
    {
        private float _curving;
        private float? _curvingDuration;
        private bool _initialized;

        private void Awake ()
        {
            Rb = GetComponent<Rigidbody>();
        }
        
        public void Initialize(float speed, float curving, float? curvingDuration)
        {
            Speed = speed;
            _curving = curving;
            _curvingDuration = curvingDuration;
            Rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
            _initialized = true;
        }

        private void FixedUpdate()
        {
            if (!_initialized)
                return;

            Rb.AddForce(Rb.transform.right * -_curving, ForceMode.Acceleration);
            if (Vector3.Cross(Rb.velocity, Vector3.up) != Vector3.zero)
            {
                Rb.transform.rotation = Quaternion.LookRotation(Rb.velocity, Vector3.up);
            }
            Rb.velocity = Rb.velocity.normalized * Speed;

            // Reduce curving over time
            if (_curvingDuration != null && _curving > Time.fixedDeltaTime * _curving / _curvingDuration.Value)
            {
                _curving -= Time.fixedDeltaTime * _curving / _curvingDuration.Value;
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
