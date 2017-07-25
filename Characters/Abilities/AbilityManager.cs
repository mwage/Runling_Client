using UnityEngine;

namespace Characters.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        [HideInInspector]
        public PhotonView PhotonView;
        private GameObject _shield;
        private GameObject _godMode;

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _shield = transform.Find("Shield").gameObject;
            _godMode = transform.Find("GodMode").gameObject;
        }


        [PunRPC]
        public void ToggleAbility(string ability, bool on)
        {
            switch (ability)
            {
                case "Shield":
                    _shield.SetActive(on);
                    break;
                case "GodMode":
                    _godMode.SetActive(on);
                    break;
            }
        }
    }
}