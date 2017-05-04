using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class MoveSinusoidal : MonoBehaviour
    {
        private Rigidbody rb;
        private float initializationTime;
        // Use this for initialization
        private void Start () {
            initializationTime = Time.timeSinceLevelLoad;
            rb = GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * 5, ForceMode.VelocityChange);
        }
	

        private void FixedUpdate()
        {
            rb.AddForce(rb.transform.right * 10 * Mathf.Sin( (Time.time- initializationTime )* 5), ForceMode.Acceleration);
        }
    }
}
