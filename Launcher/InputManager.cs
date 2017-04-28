using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Launcher
{
    public class InputManager : MonoBehaviour
    {
        public static Dictionary<string, KeyCode> Hotkeys = new Dictionary<string, KeyCode>();

        // Use this for initialization
        void Start ()
        {
            LoadHotkeys();
        }

        public static bool GetButtonDown(string hotkeyName)
        {
            if (!Hotkeys.ContainsKey(hotkeyName))
            {
                Debug.LogError("no hotkey named " + hotkeyName);
                return false;
            }

            return Input.GetKeyDown(Hotkeys[hotkeyName]);
        }

        public static void LoadHotkeys()
        {

            Hotkeys["Activate Autoclicker"] = PlayerPrefs.GetInt("Activate Autoclicker") != 0 
                ? (KeyCode)PlayerPrefs.GetInt("Activate Autoclicker") 
                : Hotkeys["Activate Autoclicker"] = KeyCode.LeftControl;
            Hotkeys["Deactivate Autoclicker"] = PlayerPrefs.GetInt("Deactivate Autoclicker") != 0
                ? (KeyCode) PlayerPrefs.GetInt("Deactivate Autoclicker")
                : Hotkeys["Activate Autoclicker"] = KeyCode.LeftAlt;
            Hotkeys["Navigate Menu"] = PlayerPrefs.GetInt("Navigate Menu") != 0
                ? (KeyCode)PlayerPrefs.GetInt("Navigate Menu")
                : Hotkeys["Navigate Menu"] = KeyCode.Escape;
            Hotkeys["Pause"] = PlayerPrefs.GetInt("Pause") != 0
                ? (KeyCode)PlayerPrefs.GetInt("Pause")
                : Hotkeys["Pause"] = KeyCode.Pause;
            Hotkeys["Activate Godmode"] = PlayerPrefs.GetInt("Activate Godmode") != 0
                ? (KeyCode)PlayerPrefs.GetInt("Activate Godmode")
                : Hotkeys["Activate Godmode"] = KeyCode.Alpha1;
            Hotkeys["Deactivate Godmode"] = PlayerPrefs.GetInt("Deactivate Godmode") != 0
                ? (KeyCode)PlayerPrefs.GetInt("Deactivate Godmode")
                : Hotkeys["Deactivate Godmode"] = KeyCode.Alpha2;
        }
    }
}