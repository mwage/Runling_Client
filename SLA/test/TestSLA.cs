using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TestSLA : MonoBehaviour
{
    //attach scripts
    public DroneTestSLA _droneTestSLA;
    public ScoreTestSLA _score;
    public DeathTrigger _deathTrigger;

    void Start()
    {
        //Set current Level and movespeed
        GameControl.moveSpeed = 10f;



        //load drones and spawn immunity
        _droneTestSLA.LoadDrones();
        SpawnImmunity(3f);
    }


    //set Spawnimmunity once game starts
    public void SpawnImmunity(float duration)
    {
        StartCoroutine(SpawnImmu(duration));
    }

    IEnumerator SpawnImmu(float duration)
    {
        yield return new WaitForSeconds(duration);
        _deathTrigger.gameObject.SetActive(true);
        _score.StartScore();
    }
}

