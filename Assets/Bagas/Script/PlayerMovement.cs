using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public LayerMask pushLayer;
    public Vector2 moveInput;
    public Rigidbody boxRb;
    public Vector3 boxDirTarget;
    public Vector3 playerDirection;
    public Vector3 targetPosBox;
    public Rigidbody rb;
    public NewBoxScript boxScript;
    private RaycastHit hitInfo;
    public GameObject playerSprite;
    public AudioManager audioManager;
    public GameManager gameManager;

    public float moveSpeed = 5f;
    public float rotationSpeed = 2;
    public float lookSensitivity = 1f;

    public bool isPush = false;
    public bool isBoxCollide = false;
    public bool pushAnim = false;
    private bool delayRunning = false;
    public bool hasMovedBox = false;
    private bool isWalkingSFXPlaying = false;
    private bool isPushingSFXPlaying = false;
    private bool isRestart = false;
    public bool isFirstFrame = false;
    public bool hasPressed = false;
    public float delayToPush = 0.4f;
    private float timerToPush = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (boxRb != null && pushAnim && isPush && !hasMovedBox) {
            TargetMoveBox();
            hasMovedBox = true;
            transform.SetParent(null);
            StartCoroutine(ResetIsPush());
        }

        if (boxScript != null) {
            try {
                boxScript.MoveBox(targetPosBox);
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }
        else {
            try {
                targetPosBox = boxScript.boxFirstPos;
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }

        if (isBoxCollide && !isPush && hasMovedBox) {
            if (moveInput != Vector2.zero) {
                TryToPush();
            }
        }

        if(isRestart) {
            gameManager.Restart();
        }

        if(pushAnim && !hasPressed) {
            isFirstFrame = true;
            hasPressed = true;
        }
        else if(!pushAnim) {
            hasPressed = false;
        }

        if(isFirstFrame) {
            if (!delayRunning && pushAnim) {
                StartCoroutine(HandlePushDelay());
                targetPosBox = boxRb.transform.position;
            }
            // boxDirTarget = CreateRaycast(.3f, pushLayer);

            isFirstFrame = false;
        }

        CreateRaycast(0.3f, pushLayer);
        if(boxScript != null) {
            boxScript.boxDir = rayDir;
        }

    }

    private void FixedUpdate()
    {
        PlayerLogic(rb);

        // Cek apakah player sedang bergerak
        if (moveInput != Vector2.zero) 
        {
            // Jika SFX berjalan belum dimainkan, mulai memutar
            if (!isWalkingSFXPlaying) 
            {
                audioManager.PlaySFX(0); // Putar SFX berjalan
                isWalkingSFXPlaying = true; // Tandai bahwa SFX sedang diputar
            }
        } 
        else 
        {
            // Jika tidak ada input gerakan, hentikan SFX berjalan
            if (isWalkingSFXPlaying) 
            {
                audioManager.StopSFX(); // Hentikan SFX berjalan
                isWalkingSFXPlaying = false;
            }
        }

        // Memainkan SFX mendorong box jika player sedang mendorong box
        if (hasMovedBox) 
        {
            if (!isPushingSFXPlaying) 
            {
                audioManager.PlayMusic(0); // Putar SFX mendorong box
                isPushingSFXPlaying = true;
            }
        } 
        else 
        {
            if (isPushingSFXPlaying)
            {
                audioManager.StopSFX(); // Hentikan SFX mendorong box
                isPushingSFXPlaying = false;
            }
        }
    }

    private void TryToPush()
    {
        timerToPush += Time.deltaTime;
        if (timerToPush > delayToPush)
        {
            isPush = true;
            StartCoroutine(ResetIsPush());
        }
    }


    private void PlayerLogic(Rigidbody rb) {
        Transform cameraTransform = Camera.main.transform;

        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        playerDirection = (cameraRight * moveInput.x + cameraForward * moveInput.y).normalized;

        Vector3 moveVelocity = playerDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

        if (playerDirection == Vector3.left) {
            playerSprite.transform.eulerAngles = new Vector3(-30f, 0f, 0f);
        } else if (playerDirection == Vector3.right) {
            playerSprite.transform.eulerAngles = new Vector3(30f, 180f, 0f);
        }
    }

    private void TargetMoveBox() {
        if (boxScript.boxDir == Vector3.forward && boxScript.canPush) {
            targetPosBox += new Vector3(0f, 0f, boxScript.gridSize.z);
        } else if (boxScript.boxDir == Vector3.back && boxScript.canPush) {
            targetPosBox += new Vector3(0f, 0f, -boxScript.gridSize.z);
        } else if (boxScript.boxDir == Vector3.right && boxScript.canPush) {
            targetPosBox += new Vector3(boxScript.gridSize.x, 0f, 0f);
        } else if (boxScript.boxDir == Vector3.left && boxScript.canPush) {
            targetPosBox += new Vector3(-boxScript.gridSize.x, 0f, 0f);
        }
    }

    private IEnumerator ResetIsPush() {
        rb.transform.SetParent(boxRb.transform);
        yield return new WaitForSeconds(0.5f);
        isPush = false;
        timerToPush = 0;
        hasMovedBox = false;
        rb.transform.SetParent(null);
        // transform.position -= new Vector3(boxDirTarget.x - (97f/100f * boxDirTarget.x), boxDirTarget.y - (97f/100f * boxDirTarget.y), boxDirTarget.z - (97f/100f * boxDirTarget.z));
        yield return new WaitForSeconds(0.5f);
    
        if (playerDirection == boxScript.boxDir) {
            TryToPush();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            isBoxCollide = true;
            pushAnim = playerDirection == boxDirTarget;

            boxRb = collision.rigidbody;
            boxScript = boxRb.GetComponent<NewBoxScript>();
            SaveManager saveManager = boxRb.GetComponent<SaveManager>();

            // TextMeshProUGUI saveTest = GameObject.Find("Test").GetComponent<TextMeshProUGUI>();

            // if(PlayerPrefs.HasKey(saveManager.saveName + "X")) {
            //     saveTest.text = PlayerPrefs.GetFloat(saveManager.saveName + "X").ToString();
            // }
            // else {
            //     saveTest.text = "PlayerPrefs = null";
            // }
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            boxRb = collision.rigidbody;
            boxScript = boxRb.GetComponent<NewBoxScript>();
            // targetPosBox = boxRb.transform.position;
            pushAnim = playerDirection == boxScript.boxDir;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            pushAnim = false;
            isBoxCollide = false;
            isPush = false;
            timerToPush = 0;
            hasMovedBox = false;

            boxRb = null;
            boxScript = null;
            // targetPosBox = Vector3.zero;
        }
    }

    private IEnumerator HandlePushDelay() {
        delayRunning = true;
        yield return new WaitForSeconds(0.5f);
        isPush = true;
        delayRunning = false;
    }
    public Vector3 rayDir;
    private void CreateRaycast(float rayLength, LayerMask boxLayer) {
        Ray ray = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.forward);
        Ray ray1 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.back);
        Ray ray2 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.right);
        Ray ray3 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.left);

        if (Physics.Raycast(ray, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitForward");
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
            rayDir = ray.direction;
        }
        else if (Physics.Raycast(ray1, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitBack");
            Debug.DrawRay(ray1.origin, ray1.direction * rayLength, Color.red);
            rayDir = ray1.direction;
        }
        else if (Physics.Raycast(ray2, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitRight");
            Debug.DrawRay(ray2.origin, ray2.direction * rayLength, Color.red);
            rayDir = ray2.direction;
        }
        else if (Physics.Raycast(ray3, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitLeft");
            Debug.DrawRay(ray3.origin, ray3.direction * rayLength, Color.red);
            rayDir = ray3.direction;
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

    //----------------------------------------------------------------------------- Input ------------------------------------------------------------------------------//
    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRestart(InputAction.CallbackContext context) {
        isRestart = context.performed;
    }
}
