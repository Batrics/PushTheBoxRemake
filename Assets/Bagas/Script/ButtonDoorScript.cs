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
        // Check if the number of clicked buttons equals the total number of buttons
        if (clickedInt == buttonScripts.Count) {
            allButtonClicked = true;
        } else {
            allButtonClicked = false;
        }

        // If all buttons are clicked and the door is not open yet, play the open animation
        if (allButtonClicked && !open) {
            dOTweenAnimation.DORestart(); // Restart to make sure it resets correctly
            dOTweenAnimation.DOPlay();    // Play the animation forward
            open = true; // mark the door as open
        }
        // If not all buttons are clicked and the door is currently open, play the close animation
        else if (!allButtonClicked && open) {
            // dOTweenAnimation.DORestart(); // Restart the animation to reset it
            dOTweenAnimation.DOPlayBackwards(); // Play the animation backwards to close the door
            open = false; // mark the door as closed
        }
    }
}
