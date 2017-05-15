using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.Launcher
{
    public enum HotkeyAction
    {
        ActivateClicker, // general hotkeys
        DeactivateClicker,
        NavigateMenu,
        Pause,
        ActivateGodmode,
        DeactiveGodmode,
        CameraUp, // camera hotkeys
        CameraDown,
        CameraRight,
        CameraLeft,
        ZoomMore,
        ZoomLess,
        RotateRight,
        RotateLeft
        // graphics hotkeys

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


        private static HotkeyAction? _hotkeyToRebind;
        public static readonly Dictionary<HotkeyAction, Text> HotkeyLabel = new Dictionary<HotkeyAction, Text>();

        private static readonly Dictionary<HotkeyAction, KeyCode> Hotkeys = new Dictionary<HotkeyAction, KeyCode>();
        private static readonly Dictionary<KeyCode, HotkeyAction> KeyCodes = new Dictionary<KeyCode, HotkeyAction>();

        private InputManager()
        {
            LoadHotkeys();
            Console.WriteLine(Hotkeys.Count);
        }

        public bool GetButtonDown(HotkeyAction hotkeyAction)
        {
            if (!Hotkeys.ContainsKey(hotkeyAction))
            {
               // Debug.Log("no hotkey named " + hotkeyAction);
                return false;
            }

            return Input.GetKeyDown(Hotkeys[hotkeyAction]);
        }

        public KeyCode? GetHotkey(HotkeyAction action)
        {
            if (Hotkeys.ContainsKey(action))
                return Hotkeys[action];

            return null;
        }

        private static HotkeyAction? UpdateHotkey(HotkeyAction action, KeyCode keyCode)
        {
            HotkeyAction? removed = null;

            // Remove old hotkey mapping
            if (KeyCodes.ContainsKey(keyCode))
            {
                var oldAction = KeyCodes[keyCode];
                KeyCodes.Remove(keyCode);
                Hotkeys.Remove(oldAction);
                removed = oldAction;
            }

            // Remove old keycode mapping
            if (Hotkeys.ContainsKey(action))
                KeyCodes.Remove(Hotkeys[action]);

            Hotkeys[action] = keyCode;
            KeyCodes[keyCode] = action;

            return removed;
        }

        public static void LoadHotkey(HotkeyAction action, KeyCode defaultKeyCode)
        {
            var kc = PlayerPrefs.GetInt(action.ToString());
            if (kc != 0)
                UpdateHotkey(action, (KeyCode)kc);
            else
                UpdateHotkey(action, defaultKeyCode);
        }

        //public static void LoadSliderValue(HotkeyAction action, float defaultValue)
        //{
        //    float kc = PlayerPrefs.GetFloat(action.ToString());
        //    if (kc != 0)
        //        UpdateHotkey(action, (float)kc);
        //    else
        //        UpdateHotkey(action, defaultValue);
        //}

        public static void LoadHotkeys()
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
        
            // write lacking hotkeys if need to have them chosen
        }

        public static void RebindHotkey(HotkeyAction action)
        {
            _hotkeyToRebind = action;
        }

        public static void RebindHotkeyIfNeed()
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
                            HotkeyLabel[_hotkeyToRebind.Value].text = kc.ToString();
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


