using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool isClick;
    public string TargetElement;
    public ButtonDoorScript buttonDoorScript;

    private void Start() {
        buttonDoorScript = transform.parent.GetComponent<ButtonDoorScript>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Box")) {
            BoxElement boxElement = other.GetComponent<BoxElement>();
            if(TargetElement == boxElement.element) {
                isClick = true;
                buttonDoorScript.clickedInt++;
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Box")) {
            BoxElement boxElement = other.GetComponent<BoxElement>();
            if(TargetElement == boxElement.element) {
                isClick = false;
                buttonDoorScript.clickedInt--;
            }
        }
    }
}
