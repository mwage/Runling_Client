using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Launcher;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class SubmenuBuilder : MonoBehaviour
    {
        public static void AddButton(HotkeyAction action, GameObject setHotkeyPrefab, GameObject hotkeyList)
        {
            var hotkey = Instantiate(setHotkeyPrefab, hotkeyList.transform);
            hotkey.transform.localScale = Vector3.one;

            hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = InputManager.GetFriendlyName(action.ToString());
            var hotkeyText = hotkey.transform.Find("SetHotkey/KeyPressed").GetComponent<Text>();
            var kc = InputManager.Instance.GetHotkey(action);
            hotkeyText.text = kc != null ? kc.ToString() : "<None>";
            InputManager.HotkeyLabel[action] = hotkeyText;

            var setHotkey = hotkey.transform.Find("SetHotkey").GetComponent<Button>();
            var keyName = action;
            setHotkey.onClick.AddListener(() => { InputManager.RebindHotkey(keyName);});
        }

        public static Slider AddSlider(GameObject sliderPrefab, GameObject hotkeyList, string name, float defaultValue, float minValue, float maxValue, UnityEngine.Events.UnityAction<float> sliderFunction)
        {
            GameObject hotkey = Instantiate(sliderPrefab, hotkeyList.transform);
            hotkey.transform.localScale = Vector3.one;

            hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = name;

            Slider slider =  hotkey.transform.FindChild("Slider").gameObject.GetComponent<Slider>();
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.onValueChanged.AddListener(sliderFunction);
            return slider;
        }

        public static void DeleteHotkeyPrefabs(GameObject hotkeyList)
        {
            foreach (Transform child in hotkeyList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public static Toggle AddSelection(GameObject selectionPrefab, GameObject hotkeyList, string name, UnityEngine.Events.UnityAction<bool> selectionFunction)
        {
            GameObject hotkey = Instantiate(selectionPrefab, hotkeyList.transform);
            hotkey.transform.localScale = Vector3.one;

            hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = name;

            Toggle selection = hotkey.transform.FindChild("Toggle").gameObject.GetComponent<Toggle>();
            selection.onValueChanged.AddListener(selectionFunction);
            return selection;
        }
    }
}
