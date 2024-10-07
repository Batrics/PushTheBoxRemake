using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool isClick;
    public ButtonDoorScript buttonDoorScript;

    private void Start() {
        buttonDoorScript = transform.parent.GetComponent<ButtonDoorScript>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Box")) {
            isClick = true;
            buttonDoorScript.clickedInt++;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Box")) {
            isClick = false;
            buttonDoorScript.clickedInt--;
        }
    }
}
