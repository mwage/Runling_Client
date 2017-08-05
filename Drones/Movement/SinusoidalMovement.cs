using UnityEngine;

namespace Drones.Movement
{
    public class SinusoidalMovement : ADroneMovement
    {
        private Rigidbody _rb;
        private float _offset;
        private GameObject _droneModel;
        public float SinFrequency;
        public float SinForce;
        public float DroneSpeed;
        public bool Fixed;

        private DroneManager _droneManager;

        private float _offsetAtFreeze;
        private float _currentSinForce;
        private float _currentSinFrequency;
        private Vector3 _veloctiyBeforeFreeze;

        public bool Initialized;


        private void Start ()
        {
            while (!Initialized) // wait for writing walues for SinForce, Speed..
            {
                
            }
            _offset = Time.time;
            _rb = GetComponent<Rigidbody>();
            _droneModel = transform.Find("Model").gameObject;
            _currentSinForce = SinForce;
            _currentSinFrequency = SinFrequency;
            
        }

        private void FixedUpdate()
        {
        }

        public override void Move()
        {
            //if (Initialized)
            //{
            //    _currentSinForce = SinForce;
            //    _currentSinFrequency = SinFrequency;  
            //}
            _rb.AddForce(_rb.transform.right * _currentSinForce * Mathf.Cos((Time.time - _offset) * _currentSinFrequency), ForceMode.Acceleration);
            if (Fixed)
            {
                _rb.velocity = _rb.velocity.normalized * DroneSpeed; //// ?????????????
            }
           // _droneModel.transform.LookAt(_rb.position + _rb.velocity);
        }

        /// <summary>
        /// Use it to freeze drone. it changes SinForce to 0, remebers rb.velocity value and sets it to 0. Use Unfreeze() to resume drone's movement.
        /// </summary>
        public override void Freeze()
        {
            _offsetAtFreeze = Time.time - _offset;
            _currentSinForce = 0F;
            _currentSinFrequency = 0F;
            _veloctiyBeforeFreeze = _rb.velocity;
            _rb.velocity = Vector3.zero;
        }

        public override void UnFreeze()
        {
            _offset = Time.time + _offsetAtFreeze;
            _currentSinForce = SinForce;
            _currentSinFrequency = SinFrequency;
            _rb.velocity = _veloctiyBeforeFreeze;
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
