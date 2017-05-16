using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.Launcher
{
    public enum HotkeyAction
    {
        // general hotkeys
        ActivateClicker,
        DeactivateClicker,
        NavigateMenu,
        Pause,
        ActivateGodmode,
        DeactiveGodmode,

        // camera hotkeys
        CameraUp,
        CameraDown,
        CameraRight,
        CameraLeft,
        ZoomMore,
        ZoomLess,
        RotateRight,
        RotateLeft,
        ActivateFollow
        // graphics hotkeys
    }

    public class InputManager
    {
        private HotkeyAction? _hotkeyToRebind;
        public readonly Dictionary<HotkeyAction, Text> HotkeyLabel = new Dictionary<HotkeyAction, Text>();

        private readonly Dictionary<HotkeyAction, KeyCode> _hotkeys = new Dictionary<HotkeyAction, KeyCode>();
        private readonly Dictionary<KeyCode, HotkeyAction> _keyCodes = new Dictionary<KeyCode, HotkeyAction>();

        public InputManager()
        {
            LoadHotkeys();
        }

        public bool GetButtonDown(HotkeyAction hotkeyAction)
        {
            if (!_hotkeys.ContainsKey(hotkeyAction))
            {
               // Debug.Log("no hotkey named " + hotkeyAction);
                return false;
            }

            return Input.GetKeyDown(_hotkeys[hotkeyAction]);
        }

        public KeyCode? GetHotkey(HotkeyAction action)
        {
            if (_hotkeys.ContainsKey(action))
                return _hotkeys[action];

            return null;
        }

        private HotkeyAction? UpdateHotkey(HotkeyAction action, KeyCode keyCode)
        {
            HotkeyAction? removed = null;

            // Remove old hotkey mapping
            if (_keyCodes.ContainsKey(keyCode))
            {
                var oldAction = _keyCodes[keyCode];
                _keyCodes.Remove(keyCode);
                _hotkeys.Remove(oldAction);
                removed = oldAction;
            }

            // Remove old keycode mapping
            if (_hotkeys.ContainsKey(action))
                _keyCodes.Remove(_hotkeys[action]);

            _hotkeys[action] = keyCode;
            _keyCodes[keyCode] = action;

            return removed;
        }

        private void LoadHotkey(HotkeyAction action, KeyCode defaultKeyCode)
        {
            var kc = PlayerPrefs.GetInt(action.ToString());
            if (kc != 0)
                UpdateHotkey(action, (KeyCode)kc);
            else
                UpdateHotkey(action, defaultKeyCode);
        }

        public void LoadHotkeys()
        {
            LoadHotkey(HotkeyAction.ActivateClicker, KeyCode.LeftControl);
            LoadHotkey(HotkeyAction.DeactivateClicker, KeyCode.LeftAlt);
            LoadHotkey(HotkeyAction.NavigateMenu, KeyCode.Escape);
            LoadHotkey(HotkeyAction.Pause, KeyCode.Pause);
            LoadHotkey(HotkeyAction.ActivateGodmode, KeyCode.Alpha1);
            LoadHotkey(HotkeyAction.DeactiveGodmode, KeyCode.Alpha2);
            LoadHotkey(HotkeyAction.CameraDown, KeyCode.S);
            LoadHotkey(HotkeyAction.CameraUp, KeyCode.W);
            LoadHotkey(HotkeyAction.CameraLeft, KeyCode.A);
            LoadHotkey(HotkeyAction.CameraRight, KeyCode.D);
            LoadHotkey(HotkeyAction.ZoomMore, KeyCode.O);
            LoadHotkey(HotkeyAction.ZoomLess, KeyCode.P);
            LoadHotkey(HotkeyAction.RotateRight, KeyCode.K);
            LoadHotkey(HotkeyAction.RotateLeft, KeyCode.L);
            LoadHotkey(HotkeyAction.ActivateFollow, KeyCode.F);
            // write lacking hotkeys if need to have them chosen
        }

        public void RebindHotkey(HotkeyAction action)
        {
            _hotkeyToRebind = action;
        }

        public void RebindHotkeyIfNeed()
        {
            if (_hotkeyToRebind != null)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(kc))
                        {
                            var removed = UpdateHotkey(_hotkeyToRebind.Value, kc);
                            if (removed != null)
                                HotkeyLabel[removed.Value].text = "<None>";
                            if (_hotkeyToRebind != null) HotkeyLabel[_hotkeyToRebind.Value].text = kc.ToString();
                            _hotkeyToRebind = null;
                            break;
                        }
                    }
                }
            }
        }

        public static string GetFriendlyName(string input)
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
    }
}
