using UnityEngine;

namespace Characters.Abilities
{
    public class AbilityToggle : MonoBehaviour
    {
        private AbilityManager _abilityManager;

        private void Initialize()
        {
            _abilityManager = transform.parent.GetComponent<AbilityManager>();
        }

        private void OnEnable()
        {
            if (_abilityManager == null)
                Initialize();
            if (_abilityManager.PhotonView.isMine)
                _abilityManager.PhotonView.RPC("ToggleAbility", PhotonTargets.Others, gameObject.name, true);
        }

        private void OnDisable()
        {
            if (_abilityManager.PhotonView.isMine)
                _abilityManager.PhotonView.RPC("ToggleAbility", PhotonTargets.Others, gameObject.name, false);
        }
    }
}