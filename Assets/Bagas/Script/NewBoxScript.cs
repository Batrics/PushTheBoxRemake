using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoxScript : MonoBehaviour
{
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public PlayerMovement playerMovement;
    public RaycastHit hitInfo;
    public Vector3 playerDir;
    public Vector3 boxDir;
    public Vector3 boxFirstPos;
    public Vector3 gridSize = new Vector3(1f, 0f, 1f); // Ukuran grid
    public bool canPush = false;
    public bool hasMovedBox = false;
    public bool fixBoxPos = true;

    private void Awake() {
        boxFirstPos = transform.position;
    }

    private void Update() {
        if(playerMovement != null) {
            hasMovedBox = playerMovement.hasMovedBox;
            fixBoxPos = true;
        }
        else {
            hasMovedBox = false;
        }

        if(!hasMovedBox && fixBoxPos) {
            fixBoxPos = false;
            SnapToGrid();
        }

        if (playerMovement != null) {
            bool isCollide = playerMovement.isBoxCollide;
            CreateBoxRaycast(isCollide, boxDir, 1f, wallLayer);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            print("iscollide");
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            playerDir = playerMovement.playerDirection;
            boxDir = playerMovement.boxDirTarget;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            print("Notcollide");
            playerMovement = null;
            playerDir = Vector3.zero;
        }
    }
    private void CreateBoxRaycast(bool isCollide, Vector3 direction, float rayLength, LayerMask wallLayer) {
        Ray ray = new Ray(new Vector3(transform.position.x + 0.5f, 0.5f, transform.position.z - 0.5f), direction);
        if (isCollide) {
            if(Physics.Raycast(ray, out hitInfo, rayLength, wallLayer, QueryTriggerInteraction.UseGlobal)) {
                Debug.Log("RaycastHit");
                Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.red);
                canPush = false;
            }
        else {
            canPush = true;
            Debug.Log("Raycast Null");
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.blue);
        }
        }
    }

    public void MoveBox(Vector3 targetPos, float moveSpeed = 5f) {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void SnapToGrid()
    {
        // Menghitung posisi snap berdasarkan ukuran grid
        Vector3 snapPosition = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z)
        );

        // Atur posisi box ke posisi snap
        transform.position = snapPosition;
    }
}
