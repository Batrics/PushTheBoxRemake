using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public LayerMask pushLayer;
    public Vector2 moveInput;
    public Rigidbody boxRb;
    public Vector3 boxDirTarget;
    public Vector3 playerDirection;
    private Rigidbody rb;

    public float moveSpeed = 5f;
    public float rotationSpeed = 2;
    public float lookSensitivity = 1f;

    public bool isPush;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        PlayerLogic(rb);
    }


    private void PlayerLogic(Rigidbody rb) {
        // Movement direction based on inputa
        playerDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 moveVelocity = playerDirection * moveSpeed;

        if (playerDirection != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            rb.rotation = targetRotation;
        }

        if(isPush) {
            MoveBox(boxRb, rb, playerDirection);
        }
        else {
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
    }
    private void MoveBox(Rigidbody boxRb, Rigidbody playerRb, Vector3 playerDir) {
        Vector3 moveDirectionBox = playerDir;
        Vector3 moveVelocityBox = moveDirectionBox * (moveSpeed - 2f);

        boxRb.velocity = new Vector3(boxDirTarget.x, 0, boxDirTarget.z);
        playerRb.velocity = new Vector3(moveVelocityBox.x, rb.velocity.y, moveVelocityBox.z);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Box")) {
            isPush = true;
            if((playerDirection.x == 0f && playerDirection.z != 0f) || (playerDirection.x != 0f && playerDirection.z == 0f)) {
                boxDirTarget = playerDirection;
            }
            else {
                boxDirTarget = Vector3.zero;
            }
            boxRb = collision.rigidbody;
            print("IsPush : " + isPush);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.CompareTag("Box")) {
            isPush = false;
            boxDirTarget = Vector3.zero;
            boxRb = null;
            print("IsPush : " + isPush);
        }
    }

}