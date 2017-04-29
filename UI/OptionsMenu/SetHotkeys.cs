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

        private HotkeyAction? _hotkeyToRebind;
        private readonly Dictionary<HotkeyAction, Text> _hotkeyLabel = new Dictionary<HotkeyAction, Text>();


        private static string GetFriendlyName(string input)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
            {
                if (char.IsUpper(c))
                    sb.Append(" ");
                sb.Append(c);
            }

            if (input.Length > 0 && char.IsUpper(input[0]))
                sb.Remove(0, 1);

            return sb.ToString();
        }

        private void OnEnable()
        {
            foreach (HotkeyAction action in Enum.GetValues(typeof(HotkeyAction)))
            {
                var hotkey = Instantiate(SetHotkeyPrefab, HotkeyList.transform);
                hotkey.transform.localScale = Vector3.one;

                hotkey.transform.Find("HotkeyName").GetComponent<Text>().text = GetFriendlyName(action.ToString());
                var hotkeyText = hotkey.transform.Find("SetHotkey/KeyPressed").GetComponent<Text>();
                var kc = InputManager.Instance.GetHotkey(action);
                hotkeyText.text = kc != null ? kc.ToString() : "<None>";
                _hotkeyLabel[action] = hotkeyText;

                var setHotkey = hotkey.transform.Find("SetHotkey").GetComponent<Button>();
                var keyName = action;
                setHotkey.onClick.AddListener(() => { RebindHotkey(keyName); });
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
                            var removed = InputManager.Instance.UpdateHotkey(_hotkeyToRebind.Value, kc);
                            if (removed != null)
                                _hotkeyLabel[removed.Value].text = "<None>";
                            _hotkeyLabel[_hotkeyToRebind.Value].text = kc.ToString();
                            _hotkeyToRebind = null;
                            break;
                        }
                    }
                }
            }
        }

        private void RebindHotkey(HotkeyAction action)
        {
            _hotkeyToRebind = action;
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
