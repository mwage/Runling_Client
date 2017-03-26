using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCoroutineSLA : MonoBehaviour {

    public AddDrone _addDrone;
    public Level4SLA _level4SLA;

    public void StopRespawn()
    {
        _addDrone.StopAllCoroutines();
        _level4SLA.StopAllCoroutines();
    }
}
