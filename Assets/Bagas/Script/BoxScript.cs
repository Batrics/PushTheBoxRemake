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
    public Vector3 boxPushTarget;
    public bool raycastHit;
    public float rayLength = 5;

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
        print(rays.Count);
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
    }

    private void BoxRay(Ray ray) {
        if(Physics.Raycast(ray, out hitInfo, rayLength, layerMask, QueryTriggerInteraction.Ignore)) {
            Debug.Log("RaycastHit");
            boxPushDir = ray.direction;

            if(boxPushDir == Vector3.forward) {
                boxPushTarget = Vector3.back;
            }
            else if(boxPushDir == Vector3.back) {
                boxPushTarget = Vector3.forward;
            }
            else if(boxPushDir == Vector3.right) {
                boxPushTarget = Vector3.left;
            }
            else if(boxPushDir == Vector3.left) {
                boxPushTarget = Vector3.right;
            }
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.white);
        }
        else {
            Debug.Log("Raycast Null");
            // boxPushDir = Vector3.zero;
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.blue);
        }
    }
}
