using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Drones;
using Assets.Scripts.Launcher;
using Assets.Scripts.RLR;
using Assets.Scripts.RLR.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI.RLR_Menus;


public class LevelManagerRLR : MonoBehaviour {

    //attach scripts
    public InGameMenuManagerRLR InGameMenuManagerRLR;
    public InitializeGameRLR InitializeGameRLR;

    public GameObject Win;

    public DroneFactory DroneFactory;
    public GenerateMapRLR GenerateMapRLR;

    //public static int NumLevels = 9;             //currently last level available in RLR
    private List<ILevelRLR> _levels;


    private void InitializeLevels()
    {
        _levels = new List<ILevelRLR>
        {
            new Level1RLR(this),
            new Level2RLR(this),
            new Level3RLR(this),
            new Level4RLR(this),
            new Level5RLR(this),
            new Level6RLR(this),
            new Level7RLR(this),
            new Level8RLR(this),
            new Level9RLR(this)
        };
    }

    public void Awake()
    {
        InitializeLevels();
    }

    //Spawn Drones according to what level is active
    public void LoadDrones(int level)
    {
        try
        {
            _levels[level - 1].CreateDrones();
        }
        catch (Exception e)
        {
            Debug.Log("Failed to load level " + level + ": " + e.Message + " - " + e.StackTrace);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GenerateMap(int level)
    {
        _levels[level - 1].GenerateMap();
    }

    // Load next level
    public void EndLevel(float delay)
    {
        GameControl.FinishedLevel = false;
        StartCoroutine((GameControl.CurrentLevel == _levels.Count) ? EndGameRLR(delay) : LoadNextLevel(0));
    }

    // End game
    public void EndGame(float delay)
    {
        StartCoroutine(EndGameRLR(delay));
    }

    // Load in all but the last level
    private IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        DroneFactory.StopAllCoroutines();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var t in enemies)
        {
            Destroy(t);
        }
        Destroy(InitializeGameRLR.Player);
        GameControl.CurrentLevel++;
        InitializeGameRLR.InitializeGame();
    }

    // Load after the last level
    private IEnumerator EndGameRLR(float delay)
    {
        // Load win screen
        yield return new WaitForSeconds(delay);
        InGameMenuManagerRLR.CloseMenus();
        Win.gameObject.SetActive(true);
    }
}
