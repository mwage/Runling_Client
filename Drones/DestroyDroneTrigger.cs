using UnityEngine;
using UnityEngine.SceneManagement;

namespace Drones
{
    public class DestroyDroneTrigger : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                    PhotonNetwork.Destroy(transform.parent.gameObject);
            }
        }
    }
}
