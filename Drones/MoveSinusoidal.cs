using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MoveSinusoidal : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _initializationTime;
        private float _speed;
        private float _frequency;
        private float _sinForce;

        private void Start ()
        {
            _sinForce = 20;
            _speed = 4;
            _frequency = 5;

            _initializationTime = Time.timeSinceLevelLoad;
            _rb = GetComponent<Rigidbody>();
            _rb.AddForce(_rb.transform.forward * _speed, ForceMode.VelocityChange);
        }
	

        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * _sinForce * Mathf.Cos( (Time.time- _initializationTime )* _frequency), ForceMode.Acceleration);
        }
    }
}
