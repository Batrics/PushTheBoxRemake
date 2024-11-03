using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool isClick;
    public string TargetElement;
    public ButtonDoorScript buttonDoorScript;

    // New field for default material color
    public Color defaultColor = Color.white; // Default color
    private Renderer buttonRenderer; // Renderer to change material color

    private void Start()
    {
        buttonDoorScript = transform.parent.GetComponent<ButtonDoorScript>();   
        buttonRenderer = GetComponent<Renderer>(); // Get the Renderer component
        if (buttonRenderer != null)
        {
            // Set the default color from the Inspector
            buttonRenderer.material.color = defaultColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            BoxElement boxElement = other.GetComponent<BoxElement>();
            if (TargetElement == boxElement.element)
            {
                AudioManager.instance.PlaySFX(2);
                isClick = true;
                buttonDoorScript.clickedInt++;

                // Change material color to white
                if (buttonRenderer != null)
                {
                    buttonRenderer.material.color = Color.white;
                }
            }
            else
            {
                AudioManager.instance.PlaySFX(3);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            BoxElement boxElement = other.GetComponent<BoxElement>();
            if (TargetElement == boxElement.element)
            {
                isClick = false;
                buttonDoorScript.clickedInt--;

                // Revert to default material color
                if (buttonRenderer != null)
                {
                    buttonRenderer.material.color = defaultColor;
                }
            }
        }
    }
}
