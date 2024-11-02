using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator playerAnimator;
    public static int stateInfo;
    private PlayerMovement playerMovement;

    private void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0); 
        stateInfo = currentState.shortNameHash;
        if(playerMovement != null) {
            
            Vector2 dir = new Vector2(playerMovement.boxDirTarget.x, playerMovement.boxDirTarget.z);
            
            if(playerMovement.moveInput != Vector2.zero) {
                playerAnimator.SetBool("Walk", true);
            }
            else {
                playerAnimator.SetBool("Walk", false);
            }
            playerAnimator.SetBool("Push", playerMovement.pushAnim);
        }
    }

}
