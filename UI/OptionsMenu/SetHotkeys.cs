using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Launcher;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class SetHotkeys : MonoBehaviour
    {
        public GameObject SetHotkeyPrefab;
        public GameObject HotkeyList;


        private void OnEnable()
        {
            if (HotkeyList.transform.childCount == 0) // dont add the same buttons when switching between submenus
            {
                SubmenuBuilder.AddButton(HotkeyAction.ActivateClicker, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.DeactivateClicker, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.NavigateMenu, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.Pause, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.ActivateGodmode, SetHotkeyPrefab, HotkeyList);
                SubmenuBuilder.AddButton(HotkeyAction.DeactiveGodmode, SetHotkeyPrefab, HotkeyList);
            }
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.RebindHotkeyIfNeed();
        }

        public void DeleteHotkeyPrefabs()
        {
            foreach (Transform child in HotkeyList.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

