using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI.OptionsMenu
{
    public class SetHotkeys : MonoBehaviour
    {
        public GameObject SetHotkeyPrefab;
        public GameObject HotkeyList;

        private string _hotkeyToRebind;
        Dictionary<string, Text> _hotkeyLabel = new Dictionary<string, Text>();

        private void Start()
        {
            foreach (string hotkeyName in InputManager.Hotkeys.Keys)
            {
                var hotkey = Instantiate(SetHotkeyPrefab, HotkeyList.transform);
                hotkey.transform.localScale = Vector3.one;

                hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = hotkeyName;
                var hotkeyText = hotkey.transform.Find("SetHotkey/KeyPressed").GetComponent<Text>();
                hotkeyText.text = InputManager.Hotkeys[hotkeyName].ToString();
                _hotkeyLabel[hotkeyName] = hotkeyText;

                var setHotkey = hotkey.transform.Find("SetHotkey").GetComponent<Button>();
                var keyName = hotkeyName;
                setHotkey.onClick.AddListener(() => {RebindHotkey(keyName);});
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_hotkeyToRebind != null)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(kc))
                        {
                            InputManager.Hotkeys[_hotkeyToRebind] = kc;
                            _hotkeyLabel[_hotkeyToRebind].text = kc.ToString();
                            _hotkeyToRebind = null;
                            break;
                        }
                    }
                }
            }
        }

        private void RebindHotkey(string hotkeyName)
        {
            Debug.Log("Hotkey: " + hotkeyName);
            _hotkeyToRebind = hotkeyName;
        }
    }
}
