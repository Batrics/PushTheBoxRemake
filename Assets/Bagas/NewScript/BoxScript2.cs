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
    public Vector3 boxFirstPos;
    [SerializeField] private MoveBox moveBox;
    [SerializeField] private PlayerMovement2 playerMovement2;
    [SerializeField] private Vector3 dir;
    private void Start() {
        targetPos = transform.position;
    }
    private void Update() {
        MoveBox(targetPos);

        boxFirstPos = transform.position;
    }
    
    private void MoveBox(Vector3 targetPos) {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
