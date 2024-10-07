using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonDoorScript : MonoBehaviour
{
    public List<ButtonScript> buttonScripts = new List<ButtonScript>();
    public int clickedInt;
    public bool allButtonClicked;
    public bool firstRender;
    public DOTweenAnimation dOTweenAnimation;
    private void Start() {
        for(int i = 0; i < gameObject.transform.childCount; i++) {
            buttonScripts.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<ButtonScript>());
        }
    }

    private void Update() {
        firstRender = false;
        if(clickedInt == buttonScripts.Count) {
            allButtonClicked = true;
            firstRender = true;
            if(firstRender) {
                dOTweenAnimation.DOPlay();
            }
            else {
                dOTweenAnimation.DOPlayBackwards();
            }
        }
        else {
            allButtonClicked = false;
        }
    }
}
