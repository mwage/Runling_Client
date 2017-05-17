using UnityEngine;

namespace Drones
{
    public class DestroyDroneTrigger : MonoBehaviour {

        public GameObject Drone;

        private void OnTriggerStay(Collider other)
        {
            Destroy(Drone);
        }
    }
}
