using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {

    public GameObject menu;

    public bool optionsMenuActive;

    public void BackToMenu()
    {
        optionsMenuActive = false;
        gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }
}
