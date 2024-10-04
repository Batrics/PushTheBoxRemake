using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorScript : MonoBehaviour
{
    public List<ButtonScript> buttonScripts = new List<ButtonScript>();
    public int clickedInt;
    public bool allButtonClicked;
    private void Start() {
        for(int i = 0; i < gameObject.transform.childCount; i++) {
            buttonScripts.Add(gameObject.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ButtonScript>());
        }
    }

    private void Update() {
        if(clickedInt == buttonScripts.Count) {
            allButtonClicked = true;
        }
    }
}
