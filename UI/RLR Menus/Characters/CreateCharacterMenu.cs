using Characters.Repositories;
using Characters.Types;
using UI.RLRMenus.Characters;
using UnityEngine;

namespace UI.RLR_Menus.Characters
{
    public class CreateCharacterMenu : MonoBehaviour
    {
        public PickCharacterMenu PickCharacterMenu;
        public GameObject ManticorePreview;
        public GameObject Manticore2Preview;

        private ICharacterRepository _characterRepository;
        private string _character;

        public void Awake()
        {
            _characterRepository = new PlayerPrefsCharacterRepository();
        }

        public void Cancel()
        {
            gameObject.SetActive(false);
            PickCharacterMenu.gameObject.SetActive(true);
        }

        public void Pick()
        {
            if (_character == null)
            {
                return;
            }
            _characterRepository.Add(PickCharacterMenu.Id, _character);
            gameObject.SetActive(false);
            PickCharacterMenu.gameObject.SetActive(true);
        }

        public void ManticorePreviewToggle()
        {
            _character = "Manticore";
            ManticorePreview.SetActive(true);
            Manticore2Preview.SetActive(false);
        }

        public void Manticore2PreviewToggle()
        {
            _character = "ManticoreX";
            ManticorePreview.SetActive(false);
            Manticore2Preview.SetActive(true);
        }

    }
}
