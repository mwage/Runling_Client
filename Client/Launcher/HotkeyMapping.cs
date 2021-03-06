﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Launcher
{
    public enum HotkeyAction
    {
        // general hotkeys
        ActivateClicker,
        DeactivateClicker,
        NavigateMenu,
        Pause,
        Ability1,
        Ability2,
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
    }

    public class HotkeyMapping
    {
        private HotkeyAction? _hotkeyToRebind;
        public Dictionary<HotkeyAction, Text> HotkeyLabel { get; } = new Dictionary<HotkeyAction, Text>();

        private readonly Dictionary<HotkeyAction, KeyCode> _hotkeys = new Dictionary<HotkeyAction, KeyCode>();
        private readonly Dictionary<KeyCode, HotkeyAction> _keyCodes = new Dictionary<KeyCode, HotkeyAction>();

        public HotkeyMapping()
        {
            LoadHotkeys();
        }

        public bool GetButtonDown(HotkeyAction hotkeyAction)
        {
            return _hotkeys.ContainsKey(hotkeyAction) && Input.GetKeyDown(_hotkeys[hotkeyAction]);
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
            LoadHotkey(HotkeyAction.Ability1, KeyCode.C);
            LoadHotkey(HotkeyAction.Ability2, KeyCode.V);
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

        // Multiplayer Input
        public readonly byte MouseClick = 0;
        public readonly byte ClickPosition = 1;
    }
}
