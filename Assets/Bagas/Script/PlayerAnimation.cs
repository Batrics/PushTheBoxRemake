using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator playerAnimator;
    private PlayerMovement playerMovement;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        if(playerMovement != null) {
            if(playerMovement.moveInput != Vector2.zero) {
                playerAnimator.SetBool("Walk", true);
            }
            else {
                playerAnimator.SetBool("Walk", false);
            }
            playerAnimator.SetBool("Push", playerMovement.isBoxCollide);
        }
    }

}
