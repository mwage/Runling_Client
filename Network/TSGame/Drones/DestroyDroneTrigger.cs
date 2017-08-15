using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones
{
    public class DestroyDroneTrigger : MonoBehaviour
    {
        public void OnSyncedTriggerEnter(TSCollision other)
        {
            if (other.gameObject.layer == 16)
                TrueSyncManager.SyncedDestroy(gameObject);
        }
    }
}
