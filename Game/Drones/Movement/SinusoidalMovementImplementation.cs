using UnityEngine;

namespace Game.Scripts.Drones.Movement
{
    public class SinusoidalMovementImplementation : ADroneMovementImplementation
    {
        private float _offset;
        private GameObject _droneModel;
        private float _sinFrequency;
        private float _sinForce;
        private bool _fixedSpeed;

        private DroneManager _droneManager;

        private float _offsetAtFreeze;
        private float _currentSinForce;
        private float _currentSinFrequency;
        private Vector3 _veloctiyBeforeFreeze;
        private bool _initialized;

        private void Awake ()
        {
            Rb = GetComponent<Rigidbody>();
            _droneModel = transform.Find("Model").gameObject;
        }

        public void Initialize(float speed, float sinForce, float sinFrequency, bool fixedSpeed)
        {
            Speed = speed;
            _sinForce = sinForce;
            _sinFrequency = sinFrequency;
            _fixedSpeed = fixedSpeed;
            _currentSinForce = _sinForce;
            _currentSinFrequency = _sinFrequency;
            _offset = Time.time;
            Rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
            _initialized = true;
        }

        // TODO: Handle Physics here, not somewhere else!!!!
        private void FixedUpdate()
        {
        }



        public override void Move()
        {
            if (!_initialized)
                return;

            Rb.AddForce(Rb.transform.right * _currentSinForce * Mathf.Cos((Time.time - _offset) * _currentSinFrequency), ForceMode.Acceleration);

            // If fixed speed: adjust velocity to the constant speed value
            if (_fixedSpeed)
            {
                Rb.velocity = Rb.velocity.normalized * Speed;
            }
            _droneModel.transform.LookAt(Rb.position + Rb.velocity);
        }

        /// <summary>
        /// Use it to freeze drone. it changes SinForce to 0, remebers rb.velocity value and sets it to 0. Use Unfreeze() to resume drone's movement.
        /// </summary>
        public override void Freeze()
        {
            _offsetAtFreeze = Time.time - _offset;
            _currentSinForce = 0F;
            _currentSinFrequency = 0F;
            _veloctiyBeforeFreeze = Rb.velocity;
            Rb.velocity = Vector3.zero;
        }

        public override void UnFreeze()
        {
            _offset = Time.time + _offsetAtFreeze;
            _currentSinForce = _sinForce;
            _currentSinFrequency = _sinFrequency;
            Rb.velocity = _veloctiyBeforeFreeze;
        }

        public override void SlowDown(float percentage)
        {
            
        }

        public override void UnSlowDown() { }

        public void SaveState()
        {
            
        }
    }
}
