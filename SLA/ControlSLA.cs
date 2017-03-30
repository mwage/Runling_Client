using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlSLA : MonoBehaviour
{
    //attach scripts
    public LevelManagerSLA _levelManager;
    public ScoreSLA _score;
    public InitializeGameSLA _initialize;
    public DeathSLA _deathSLA;


    public bool stopUpdate;


    void Start()
    {
        // Set current Level and movespeed, load drones and spawn immunity
        stopUpdate = true;
        _initialize.InitializeGame();
        //GameControl.currentLevel = 9;
    }

    //update when dead
    private void Update()
    {
        if (GameControl.dead && !stopUpdate)
        {
            _deathSLA.Death();

            //in case of highscore, save and 
            _score.SetLevelHighScore();

            //change level
            _levelManager.EndLevel(3f);

            //dont repeat above once player dead
            stopUpdate = true;
        }
    }
}

