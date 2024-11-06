using DG.Tweening;
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
    private AudioManager audioManager;

    private void Start()
    {
        StartCoroutine(InitializeButtonDoorScript());
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private IEnumerator InitializeButtonDoorScript()
    {
        // Wait until the ButtonDoorScript component is found in the parent
        while (buttonDoorScript == null)
        {
            buttonDoorScript = transform.parent.GetComponent<ButtonDoorScript>();
            yield return null; // Wait for the next frame
        }

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
            DOVirtual.DelayedCall(1.5f, () =>
            {
                if (buttonDoorScript.open == true)
                {
                    GameManager.Instance.triggerWin();
                }
            });

            if (TargetElement == boxElement.element)
            {
                audioManager.PlaySFX(2);
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
                audioManager.PlaySFX(3);
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
