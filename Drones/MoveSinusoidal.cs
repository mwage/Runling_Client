using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MoveSinusoidal : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _initializationTime;
        public float Frequency;
        public float SinForce;

        private void Start ()
        {
            _initializationTime = Time.timeSinceLevelLoad;
            _rb = GetComponent<Rigidbody>();
        }
	

        private void FixedUpdate()
        {
            _rb.AddForce(_rb.transform.right * SinForce * Mathf.Cos( (Time.time- _initializationTime )* Frequency), ForceMode.Acceleration);
        }
    }
}
