using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDroneTrigger : MonoBehaviour {

    public GameObject drone;

    void OnTriggerStay(Collider other)
    {
        Destroy(drone);
    }

}
