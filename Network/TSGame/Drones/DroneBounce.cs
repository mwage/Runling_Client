using TrueSync;
using UnityEngine;

namespace MP.TSGame.Drones
{
    public class DroneBounce : TrueSyncBehaviour
    {
        public void OnSyncedTriggerEnter(TSCollision other)
        {
            if (other.gameObject.layer != 19 && other.gameObject.layer != 16)
                return;

            var normal = other.contacts[0].normal * -1;

            if (other.gameObject.name == "Backup")
            {
                Debug.Log("needed backup");
                tsRigidBody.velocity *= -1;
            }
            else
            {
                if (TSMath.Abs(normal.x) > 0.1 && TSMath.Abs(normal.z) > 0.1)
                {
                    if (TSMath.Abs(normal.x) > TSMath.Abs(normal.z))
                    {
                        normal.z = 0;
                        normal.x = TSMath.Sign(normal.x);
                    }
                    else
                    {
                        normal.x = 0;
                        normal.z = TSMath.Sign(normal.z);
                    }
                }

                tsRigidBody.velocity = -2 * TSVector.Dot(tsRigidBody.velocity, normal) * normal +
                                           tsRigidBody.velocity;
            }
        }
    }
}
