using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Drones;
using Assets.Scripts.Launcher;
using Assets.Scripts.RLR;
using Assets.Scripts.RLR.Levels;
using Assets.Scripts.RLR.GenerateMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.UI.RLR_Menus;


public class LevelManagerRLR : MonoBehaviour {

    //attach scripts
    public InGameMenuManagerRLR InGameMenuManagerRLR;
    public InitializeGameRLR InitializeGameRLR;
    public RunlingChaser RunlingChaser;
    public GameObject Win;
    public DroneFactory DroneFactory;
    public GenerateMapRLR GenerateMapRLR;

    private readonly InitializeLevelsRLR _initializeLevelsRLR = new InitializeLevelsRLR();

    //public static int NumLevels = 9;             //currently last level available in RLR
    private List<ILevelRLR> _levels;


    public void Awake()
    {
        //GameControl.SetDifficulty = GameControl.Difficulty.Hard;
        _levels = _initializeLevelsRLR.SetDifficulty(this);
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

    public void GenerateChasers(int level)
    {
        RunlingChaser.GetTriggerInstance();
        RunlingChaser.GetSafeZones();
        _levels[level - 1].SetChasers();
    }

    // Load next level
    public void EndLevel(float delay)
    {
        GameControl.Instance.State.FinishedLevel = false;
        StartCoroutine((GameControl.Instance.State.CurrentLevel == _levels.Count) ? EndGameRLR(delay) : LoadNextLevel(0));
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
        Destroy(GameControl.Instance.State.Player);
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var strongEnemies = GameObject.FindGameObjectsWithTag("Strong Enemy");
        foreach (var t in enemies)
        {
            Destroy(t);
        }
        foreach (var t in strongEnemies)
        {
            Destroy(t);
        }
        GameControl.Instance.State.CurrentLevel++;
        InitializeGameRLR.InitializeGame();
    }

    // Load after the last level
    private IEnumerator EndGameRLR(float delay)
    {
        if (!GameControl.Instance.State.IsDead)
        {
            Win.transform.Find("Victory").gameObject.SetActive(true);
            Win.transform.Find("Defeat").gameObject.SetActive(false);
        }

        // Load win screen
        yield return new WaitForSeconds(delay);
        InGameMenuManagerRLR.CloseMenus();
        Win.gameObject.SetActive(true);
    }
}
