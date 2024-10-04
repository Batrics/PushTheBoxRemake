using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public LayerMask layerMask;
    public RaycastHit hitInfo;
    private List<Ray> rays = new List<Ray>();
    private Vector3 boxPushDir;
    public Vector3 boxPushTargetDir;
    public bool raycastHit;
    public float rayLength = 5;


    public float moveSpeed = 5f; // Kecepatan perpindahan antar grid
    public Vector3 gridSize = new Vector3(1f, 0f, 1f); // Ukuran grid
    private Vector3 targetPos; // Posisi tujuan player

    private void Start() {
        print(rays.Count);
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());
        rays.Add(new Ray());

        targetPos = transform.position;
    }
    private void Update() {
        ///Forward
        rays[0] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), transform.TransformDirection(Vector3.forward)); // Tengah
        rays[1] = new Ray(new Vector3(transform.position.x + transform.localScale.x * 0.25f, 0.5f, transform.position.z), transform.TransformDirection(Vector3.forward)); // Kanan
        rays[2] = new Ray(new Vector3(transform.position.x - transform.localScale.x * 0.25f, 0.5f, transform.position.z), transform.TransformDirection(Vector3.forward)); // Kiri

        ///Back
        rays[3] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), transform.TransformDirection(Vector3.back)); // Tengah
        rays[4] = new Ray(new Vector3(transform.position.x + transform.localScale.x * 0.25f, 0.5f, transform.position.z), transform.TransformDirection(Vector3.back)); // Kanan
        rays[5] = new Ray(new Vector3(transform.position.x - transform.localScale.x * 0.25f, 0.5f, transform.position.z), transform.TransformDirection(Vector3.back)); // Kiri

        ///Right
        rays[6] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), transform.TransformDirection(Vector3.right)); // Tengah
        rays[7] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z + transform.localScale.z * 0.25f), transform.TransformDirection(Vector3.right)); // Atas
        rays[8] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z - transform.localScale.z * 0.25f), transform.TransformDirection(Vector3.right)); // Bawah

        ///Left
        rays[9] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), transform.TransformDirection(Vector3.left)); // Tengah
        rays[10] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z + transform.localScale.z * 0.25f), transform.TransformDirection(Vector3.left)); // Atas
        rays[11] = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z - transform.localScale.z * 0.25f), transform.TransformDirection(Vector3.left)); // Bawah

        foreach (var ray in rays){
            BoxRay(ray);
        }

        // if (Input.GetKeyDown(KeyCode.W)) // Ke atas
        // {
        //     targetPos += new Vector3(0f, 0f, gridSize.z);
        // }
        // if (Input.GetKeyDown(KeyCode.S)) // Ke bawah
        // {
        //     targetPos += new Vector3(0f, 0f, -gridSize.z);
        // }
        // if (Input.GetKeyDown(KeyCode.A)) // Ke kiri
        // {
        //     targetPos += new Vector3(-gridSize.x, 0f, 0f);
        // }
        // if (Input.GetKeyDown(KeyCode.D)) // Ke kanan
        // {
        //     targetPos += new Vector3(gridSize.x, 0f, 0f);
        // }

        // MoveBox(targetPos, gameObject);

    }

    public void MoveBox(Vector3 targetPos, GameObject playerG, float moveSpeed = 5f) {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void BoxRay(Ray ray) {
        if(Physics.Raycast(ray, out hitInfo, rayLength, layerMask, QueryTriggerInteraction.Collide)) {
            Debug.Log("RaycastHit");
            boxPushDir = ray.direction;

            if(boxPushDir == Vector3.forward) {
                boxPushTargetDir = Vector3.back;
            }
            else if(boxPushDir == Vector3.back) {
                boxPushTargetDir = Vector3.forward;
            }
            else if(boxPushDir == Vector3.right) {
                boxPushTargetDir = Vector3.left;
            }
            else if(boxPushDir == Vector3.left) {
                boxPushTargetDir = Vector3.right;
            }
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.white);
        }
        else {
            Debug.Log("Raycast Null");
            boxPushDir = Vector3.zero;
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.blue);
        }
    }
}
