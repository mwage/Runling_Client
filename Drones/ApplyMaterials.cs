using Drones.DroneTypes;
using Launcher;
using UnityEngine;

namespace Drones
{ 
    public class ApplyMaterials : MonoBehaviour
    {
        [HideInInspector]
        public PhotonView PhotonView;
        private DroneFactory _droneFactory;

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _droneFactory = GameObject.Find("Drone Manager(Clone)").GetComponent<DroneFactory>();
            transform.SetParent(_droneFactory.transform);
            GameControl.MapState.DronesParent = _droneFactory.gameObject;
        }

        [PunRPC]
        public void ChangeColorAndSize(DroneColor color, float size)
        {
            var model = transform.GetChild(0);
            foreach (Transform child in model)
            {
                if (child.name == "Top") continue;
                if (child.name == "Sphere")
                {
                    foreach (Transform ch in child)
                    {
                        ch.GetComponent<Renderer>().material = _droneFactory.SetDroneMaterial[color];
                    }
                }
                child.GetComponent<Renderer>().material = _droneFactory.SetDroneMaterial[color];
            }
            transform.localScale = size * Vector3.one;
        }
    }
}
