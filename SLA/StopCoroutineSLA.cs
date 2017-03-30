using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCoroutineSLA : MonoBehaviour {

    public AddDrone _addDrone;
    public SpawnDrone _spawnDrone;
    public Chaser _chaser;
    public Level4SLA _level4SLA;
    public Level6SLA _level6SLA;
    public Level7SLA _level7SLA;
    public Level8SLA _level8SLA;
    public Level9SLA _level9SLA;


    public void StopRespawn()
    {
        _addDrone.StopAllCoroutines();
        _spawnDrone.StopAllCoroutines();
        _chaser.StopAllCoroutines();
        _level4SLA.StopAllCoroutines();
        _level6SLA.StopAllCoroutines();
        _level7SLA.StopAllCoroutines();
        _level8SLA.StopAllCoroutines();
        _level9SLA.StopAllCoroutines();
    }
}
