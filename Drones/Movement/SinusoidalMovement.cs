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

        private void Start ()
        {
            _initializationTime = Time.timeSinceLevelLoad;
            _rb = GetComponent<Rigidbody>();
        }
	
        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * SinForce * Mathf.Cos( (Time.time - _initializationTime )* SinFrequency), ForceMode.Acceleration);
            //_rb.velocity = _rb.velocity.normalized * DroneSpeed;   // not sure how it is in runling
        }
    }
}
