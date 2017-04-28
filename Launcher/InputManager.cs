using System.Collections.Generic;
using UnityEngine;

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
        if (PlayerPrefs.GetInt("Activate Autoclicker") != 0)
        {
            Hotkeys["Activate Autoclicker"] = (KeyCode)PlayerPrefs.GetInt("Activate Autoclicker");
        }
        else
        {
            Hotkeys["Activate Autoclicker"] = KeyCode.LeftControl;

        }
        Hotkeys["Deactivate Autoclicker"] = KeyCode.LeftAlt;
        Hotkeys["Activate Godmode"] = KeyCode.Alpha1;
        Hotkeys["Deactivate Godmode"] = KeyCode.Alpha2;
    }
         
}