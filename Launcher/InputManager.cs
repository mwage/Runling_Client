using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Launcher
{
    public enum HotkeyAction
    {
        ActivateClicker,
        DeactivateClicker,
        NavigateMenu,
        Pause,
        ActivateGodmode,
        DeactiveGodmode
    }

    public class InputManager
    {
        private static InputManager _instance;

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InputManager();
                return _instance;
            }
        }

        private readonly Dictionary<HotkeyAction, KeyCode> _hotkeys = new Dictionary<HotkeyAction, KeyCode>();
        private readonly Dictionary<KeyCode, HotkeyAction> _keyCodes = new Dictionary<KeyCode, HotkeyAction> ();

        private InputManager()
        {
            LoadHotkeys();
            Console.WriteLine(_hotkeys.Count);
        }

        public bool GetButtonDown(HotkeyAction hotkeyAction)
        {
            if (!_hotkeys.ContainsKey(hotkeyAction))
            {
                Debug.LogError("no hotkey named " + hotkeyAction);
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

        public HotkeyAction? UpdateHotkey(HotkeyAction action, KeyCode keyCode)
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
                UpdateHotkey(action, (KeyCode) kc);
            else
                UpdateHotkey(action, defaultKeyCode);
        }
        private void LoadHotkeys()
        {
            LoadHotkey(HotkeyAction.ActivateClicker, KeyCode.LeftControl);
            LoadHotkey(HotkeyAction.DeactivateClicker, KeyCode.LeftAlt);
            LoadHotkey(HotkeyAction.NavigateMenu, KeyCode.Escape);
            LoadHotkey(HotkeyAction.Pause, KeyCode.Pause);
            LoadHotkey(HotkeyAction.ActivateGodmode, KeyCode.Alpha1);
            LoadHotkey(HotkeyAction.DeactiveGodmode, KeyCode.Alpha2);
        }
    }
}