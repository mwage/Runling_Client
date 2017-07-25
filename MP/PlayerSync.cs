using Players;
using UnityEngine;

namespace MP
{
    public class PlayerSync : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private Vector3 _networkPos;
        private Quaternion _networkRot;
        private Vector3 _networkVel;
        private double _lastPackageSent;
        private Rigidbody _rb;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _rb = _playerMovement.Rb;
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(_rb.position);
                stream.SendNext(_rb.rotation);
                stream.SendNext(_rb.velocity);
            }
            else
            {
                _networkPos = (Vector3) stream.ReceiveNext();
                _networkRot = (Quaternion) stream.ReceiveNext();
                _networkVel = (Vector3) stream.ReceiveNext();

                _lastPackageSent = info.timestamp;
            }
        }

        public void NetworkRotation()
        {
            _rb.transform.rotation = Quaternion.RotateTowards(_rb.transform.rotation, _networkRot,
                _playerMovement.RotationSpeed * 30 * Time.deltaTime);
        }

        public void NetworkPosition()
        {
            var timeSinceUpdate = (float)(PhotonNetwork.time - _lastPackageSent) + PhotonNetwork.GetPing() / 1000f;
            var estimatedPos = _networkPos +  timeSinceUpdate * _networkVel;
            var newPos = Vector3.MoveTowards(_rb.position, estimatedPos, _networkVel.magnitude * 1.1f * Time.deltaTime);

            if (Vector3.Distance(_rb.position, estimatedPos) > 0.5f * _networkVel.magnitude)
            {
                transform.position = estimatedPos;
                Debug.Log("Player too out of sync, force resync");
            }
            else
            {
                transform.position = newPos;
            }
        }
    }
}
