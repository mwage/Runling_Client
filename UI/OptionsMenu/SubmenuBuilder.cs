using Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace UI.OptionsMenu
{
    public class SubmenuBuilder
    {
        public static void AddButton(HotkeyAction action, GameObject setHotkeyPrefab, GameObject hotkeyList)
        {
            var hotkey = Object.Instantiate(setHotkeyPrefab, hotkeyList.transform);
            hotkey.transform.localScale = Vector3.one;

            hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = action.ToString().GetFriendlyName();
            var hotkeyText = hotkey.transform.Find("SetHotkey/KeyPressed").GetComponent<Text>();
            var kc = GameControl.InputManager.GetHotkey(action);
            hotkeyText.text = kc != null ? kc.ToString() : "<None>";
            GameControl.InputManager.HotkeyLabel[action] = hotkeyText;

            var setHotkey = hotkey.transform.Find("SetHotkey").GetComponent<Button>();
            var keyName = action;
            setHotkey.onClick.AddListener(() => { GameControl.InputManager.RebindHotkey(keyName);});
        }

        public static Slider AddSlider(GameObject sliderPrefab, GameObject hotkeyList, string name, float minValue, float maxValue, UnityEngine.Events.UnityAction<float> sliderFunction)
        {
            var hotkey = Object.Instantiate(sliderPrefab, hotkeyList.transform);
            hotkey.transform.localScale = Vector3.one;

            hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = name;

            var slider =  hotkey.transform.Find("Slider").gameObject.GetComponent<Slider>();
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.onValueChanged.AddListener(sliderFunction);
            return slider;
        }

        public static void DeleteHotkeyPrefabs(GameObject hotkeyList)
        {
            foreach (Transform child in hotkeyList.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static Toggle AddSelection(GameObject selectionPrefab, GameObject hotkeyList, string name, UnityEngine.Events.UnityAction<bool> selectionFunction)
        {
            var hotkey = Object.Instantiate(selectionPrefab, hotkeyList.transform);
            hotkey.transform.localScale = Vector3.one;

            hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = name;

            var selection = hotkey.transform.Find("Toggle").gameObject.GetComponent<Toggle>();
            selection.onValueChanged.AddListener(selectionFunction);
            return selection;
        }
    }
}
