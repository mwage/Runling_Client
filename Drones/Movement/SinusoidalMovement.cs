using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class SinusoidalMovement : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _initializationTime;
        public float SinFrequency;
        public float SinForce;
        public float DroneSpeed;
        public bool Fixed = false;
        public float Offset = 0;

        private void Start ()
        {
            _initializationTime = Time.time;
            _rb = GetComponent<Rigidbody>();
        }
	
        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * SinForce * Mathf.Sin(Offset + (Time.time - _initializationTime )* SinFrequency), ForceMode.Acceleration);

            if (Fixed)
            {
                _rb.velocity = _rb.velocity.normalized * DroneSpeed; 
            }
        }
    }
}
