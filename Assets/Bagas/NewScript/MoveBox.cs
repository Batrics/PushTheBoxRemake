using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public bool isPush;
    public bool canPush = true;
    public bool isFirstFrame = true;
    public LayerMask pushLayer;
    public LayerMask wallLayer;
    public Vector3 targetBoxPos;
    public Vector3 rayDir;
    private PlayerMovement2 playerMovement2;
    public RaycastHit hitInfo;
    [SerializeField] private BoxScript2 boxScript2;

    private void Start() {
        playerMovement2 = GetComponent<PlayerMovement2>();
    }

    private void Update() {
        if (isPush && isFirstFrame) {
            isFirstFrame = false;
            StartCoroutine(PushTheBox());
        }

        CreateRaycast(0.35f, pushLayer);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            boxScript2 = collision.gameObject.GetComponent<BoxScript2>();
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            if(playerMovement2.moveDirectionRelativeToCamera == rayDir) {
                if(canPush) {
                    isPush = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            isPush = false;
            isFirstFrame = true;
            transform.SetParent(null); // Pastikan keluar dari parent saat tidak mendorong
        }
    }

    private IEnumerator PushTheBox() {
        if (isPush) {
            transform.SetParent(boxScript2.transform);  // Set player sebagai child dari box saat mendorong
            yield return new WaitForSecondsRealtime(0.75f);
            
            boxScript2.targetPos += rayDir;  // Pindahkan box ke posisi target
            yield return new WaitForSecondsRealtime(0.25f);  // Tambahkan sedikit delay untuk memastikan pergerakan selesai
            
            transform.SetParent(null);  // Keluar dari parent setelah mendorong selesai
            isFirstFrame = true;
            isPush = false;
        }
    }

    private void CreateRaycast(float rayLength, LayerMask boxLayer) {

        Ray ray = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), playerMovement2.cameraTransform.forward);
        Ray ray1 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), -playerMovement2.cameraTransform.forward);
        Ray ray2 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), playerMovement2.cameraTransform.right);
        Ray ray3 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), -playerMovement2.cameraTransform.right);

        if (Physics.Raycast(ray, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            ray.origin += playerMovement2.cameraTransform.forward +  playerMovement2.cameraTransform.forward / 18f;
            Debug.Log("RaycastHitForward");
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
            rayDir = ray.direction;
            canPush = true;

            if(Physics.Raycast(ray, out hitInfo, rayLength, wallLayer, QueryTriggerInteraction.UseGlobal)) {
                Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.white);
                canPush = false;
            }
        }
        else if (Physics.Raycast(ray1, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            ray1.origin += -playerMovement2.cameraTransform.forward + -playerMovement2.cameraTransform.forward / 18f;
            Debug.Log("RaycastHitBack");
            Debug.DrawRay(ray1.origin, ray1.direction * rayLength, Color.red);
            rayDir = ray1.direction;
            canPush = true;
            
            if(Physics.Raycast(ray1, out hitInfo, rayLength, wallLayer, QueryTriggerInteraction.UseGlobal)) {
                Debug.DrawRay(ray1.origin, ray1.direction * rayLength, Color.white);
                canPush = false;
            }
        }
        else if (Physics.Raycast(ray2, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            ray2.origin += playerMovement2.cameraTransform.right +  playerMovement2.cameraTransform.right / 18f;
            Debug.Log("RaycastHitRight");
            Debug.DrawRay(ray2.origin, ray2.direction * rayLength, Color.red);
            rayDir = ray2.direction;
            canPush = true;

            if(Physics.Raycast(ray2, out hitInfo, rayLength, wallLayer, QueryTriggerInteraction.UseGlobal)) {
                Debug.DrawRay(ray2.origin, ray2.direction * rayLength, Color.white);
                canPush = false;
            }
        }
        else if (Physics.Raycast(ray3, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            ray3.origin += -playerMovement2.cameraTransform.right + -playerMovement2.cameraTransform.right / 18f;
            Debug.Log("RaycastHitLeft");
            Debug.DrawRay(ray3.origin, ray3.direction * rayLength, Color.red);
            rayDir = ray3.direction;
            canPush = true;

            if(Physics.Raycast(ray3, out hitInfo, rayLength, wallLayer, QueryTriggerInteraction.UseGlobal)) {
                Debug.DrawRay(ray3.origin, ray3.direction * rayLength, Color.white);
                canPush = false;
            }
        }
        else
        {
            Debug.Log("Raycast Null");
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.blue);
            Debug.DrawRay(ray1.origin, ray1.direction * rayLength, Color.blue);
            Debug.DrawRay(ray2.origin, ray2.direction * rayLength, Color.blue);
            Debug.DrawRay(ray3.origin, ray3.direction * rayLength, Color.blue);
            rayDir = Vector3.zero;
        }
    }
}
