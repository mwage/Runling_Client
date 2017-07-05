using Characters.Repositories;
using Characters.Types;
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
            PickCharacterMenu.SetId(null);
            PickCharacterMenu.UnselectAllSlots();
        }

        public void Pick()
        {
            if (_character == null)
            {
                return;
            }
            _characterRepository.Add(PickCharacterMenu.PickedSlot, _character);
            
            PickCharacterMenu.gameObject.SetActive(true);
            PickCharacterMenu.SetId(PickCharacterMenu.PickedSlot);
            // PickCharacterMenu.ActiveSlot.isOn = true;
            PickCharacterMenu.ActiveSlot.Select();
            gameObject.SetActive(false);
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
