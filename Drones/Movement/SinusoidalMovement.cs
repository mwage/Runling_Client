using UnityEngine;

namespace Drones.Movement
{
    public class SinusoidalMovement : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _initializationTime;
        private GameObject _droneModel;
        public float SinFrequency;
        public float SinForce;
        public float DroneSpeed;
        public bool Fixed;


        private void Start ()
        {
            _initializationTime = Time.time;
            _rb = GetComponent<Rigidbody>();
            _droneModel = transform.Find("Model").gameObject;
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * SinForce * Mathf.Cos((Time.time - _initializationTime )* SinFrequency), ForceMode.Acceleration);

            if (Fixed)
            {
                _rb.velocity = _rb.velocity.normalized * DroneSpeed; 
            }
            _droneModel.transform.LookAt(_rb.position + _rb.velocity);

        }
    }
}
