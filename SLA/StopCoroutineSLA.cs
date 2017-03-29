using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCoroutineSLA : MonoBehaviour {

    public AddDrone _addDrone;
    public Level4SLA _level4SLA;
    public Level6SLA _level6SLA;
    public Level7SLA _level7SLA;
    public Level8SLA _level8SLA;

    public void StopRespawn()
    {
        _addDrone.StopAllCoroutines();
        _level4SLA.StopAllCoroutines();
        _level6SLA.StopAllCoroutines();
        _level7SLA.StopAllCoroutines();
        _level8SLA.StopAllCoroutines();
    }
}
