using UnityEngine;

namespace Assets.Scripts.Drones
{
    public class DestroyDroneTrigger : MonoBehaviour {

        public GameObject drone;

        void OnTriggerStay(Collider other)
        {
            Destroy(drone);
        }
    }
}
