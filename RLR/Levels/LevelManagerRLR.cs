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
    public BoundariesRLR Area;

    public static int NumLevels = 13;             //currently last level available in RLR
    private List<ILevelRLR> _levels;

    private void InitializeLevels()
    {
        _levels = new List<ILevelRLR>
        {
            new Level1RLR(this),
            //new Level2RLR(this),
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

    public float GetMovementSpeed(int level)
    {
        return _levels[level - 1].GetMovementSpeed();
    }
    
    //Load next level or end game
    public void EndLevel(float delay)
    {
        StartCoroutine((GameControl.CurrentLevel == _levels.Count) ? EndGameRLR(delay) : NextLevel(delay));
    }

    //load in all but the last level
    private IEnumerator NextLevel(float delay)
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

    //load after the last level
    private IEnumerator EndGameRLR(float delay)
    {                
        //load win screen
        yield return new WaitForSeconds(delay);
        InGameMenuManagerRLR.CloseMenus();
        Win.gameObject.SetActive(true);
    }
}
