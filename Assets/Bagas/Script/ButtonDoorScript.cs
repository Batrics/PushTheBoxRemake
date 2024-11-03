using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonDoorScript : MonoBehaviour
{
    public List<ButtonScript> buttonScripts = new List<ButtonScript>();
    public int clickedInt;
    public bool allButtonClicked;
    public bool open; // flag to track if the door is open or not
    public DOTweenAnimation dOTweenAnimation; // single animation

    private void Start() {
        // Add all ButtonScript components from the children of this GameObject
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            buttonScripts.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<ButtonScript>());
        }
    }

    private void Update() {
        if (clickedInt == buttonScripts.Count) {
            allButtonClicked = true;
        } else {
            allButtonClicked = false;
        }
        DOVirtual.DelayedCall(1.5f, () =>
        {
            if (allButtonClicked && !open)
            {

                open = true; // mark the door as open
            }
            else if (!allButtonClicked && open)
            {
                open = false; // mark the door as closed
            }
            GameManager.Instance.triggerWin();
        });

    }
}
