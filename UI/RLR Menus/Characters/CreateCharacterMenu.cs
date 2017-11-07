using UnityEngine;

namespace UI.RLR_Menus.Characters
{
    public class CreateCharacterMenu : MonoBehaviour
    {
        public PickCharacterMenu PickCharacterMenu;
        public GameObject ManticorePreview;
        public GameObject Manticore2Preview;

        private string _character = "Cat";

        public void Cancel()
        {
            PickCharacterMenu.PickedSlot = 0;
            PickCharacterMenu.UnselectAllSlots();
            PickCharacterMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Pick()
        {
            PickCharacterMenu.CharacterRepository.Add(PickCharacterMenu.PickedSlot, _character);
            PickCharacterMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ManticorePreviewToggle()
        {
            _character = "Manticore";
            ManticorePreview.SetActive(true);
            Manticore2Preview.SetActive(false);
        }

        public void CatPreviewToggle()
        {
            _character = "Cat";
            ManticorePreview.SetActive(false);
            Manticore2Preview.SetActive(true);
        }
    }
}
