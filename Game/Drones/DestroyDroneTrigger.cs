using UnityEngine;

namespace Game.Scripts.Drones
{
    public class DestroyDroneTrigger : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}