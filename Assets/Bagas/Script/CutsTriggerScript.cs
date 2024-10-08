using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsTriggerScript : MonoBehaviour
{
    public GameObject thisCuts;
    public GameObject nextCuts;
    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Cuts")) {
            thisCuts.SetActive(false);
            nextCuts.SetActive(true);
        }
    }
}
