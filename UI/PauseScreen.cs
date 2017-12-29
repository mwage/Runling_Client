using UnityEngine;

namespace UI
{
    public class PauseScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _greyout;

        private MenuManager _menuManager;

        private void Awake()
        {
            _menuManager = transform.parent.parent.GetComponent<MenuManager>();
        }

        private void OnEnable()
        {
            _menuManager.GamePaused = true;
            _greyout.SetActive(true);
        }

        private void OnDisable()
        {
            _menuManager.GamePaused = false;

            if (_menuManager.ActiveMenu == null)
            {
                _greyout.SetActive(false);
            }
        }
    }
}
