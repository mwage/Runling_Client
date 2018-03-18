using Game.Scripts.Network.Data;
using UnityEngine;

namespace Game.Scripts.Network
{
    public class PlayerStateManager : MonoBehaviour
    {
        public uint Id { get; set; }

        private Vector3 _lastSentPosition;
        
        private void Awake()
        {
            _lastSentPosition = transform.position;
        }

        public PlayerState GetPositionData(float minMoveDistance)
        {
            if (Vector3.Distance(transform.position, _lastSentPosition) > minMoveDistance)
            {
                _lastSentPosition = transform.position;
                return new PlayerState(Id, _lastSentPosition.x, _lastSentPosition.z, transform.eulerAngles.y);
            }
            return null;
        }

        public void UpdatePosition(float posX, float posZ, float rotation)
        {
            // TODO: Lerp
            transform.position = new Vector3(posX, transform.position.y, posZ);
            transform.eulerAngles = new Vector3(transform.rotation.x, rotation, transform.rotation.z);
        }
    }
}
