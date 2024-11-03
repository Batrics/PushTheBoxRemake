using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxScript2 : MonoBehaviour
{
    public LayerMask wallLayer;
    public Vector3 targetPos;
    public RaycastHit hitInfo;
    public float speed;
    [SerializeField] private MoveBox moveBox;
    [SerializeField] private PlayerMovement2 playerMovement2;
    [SerializeField] private Vector3 dir;
    private void Start() {
        targetPos = transform.position;
    }
    private void Update() {
        MoveBox(targetPos);

        // if(moveBox != null && moveBox.rayDir != Vector3.zero) {
        //     CreateBoxRaycast(moveBox.rayDir, 1f, wallLayer);
        // }
    }
    
    private void MoveBox(Vector3 targetPos) {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    // private void CreateBoxRaycast(Vector3 direction, float rayLength, LayerMask wallLayer) {
    //     Ray ray = new Ray(new Vector3(transform.position.x - 0.5f, 0.5f, transform.position.z - 0.5f), direction);
    //     if(Physics.Raycast(ray, out hitInfo, rayLength, wallLayer, QueryTriggerInteraction.UseGlobal)) {
    //         Debug.Log("RaycastHit");
    //         Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.red);
    //         canPush = false;
    //     }
    //     else {
    //         Debug.Log("Raycast Null");
    //         Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.blue);
    //         canPush = true;
    //     }
    // }
}
