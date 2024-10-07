using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Cinemachine;

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
    private NewBoxScript boxScript;
    private RaycastHit hitInfo;

    public float moveSpeed = 5f;
    public float rotationSpeed = 2;
    public float lookSensitivity = 1f;
    // public float X;
    // public float Y;
    // public float Z;

    public bool isPush = false;
    public bool isBoxCollide = false;
    private bool delayRunning = false;
    private bool hasMovedBox = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Panggil MoveBox hanya jika isBoxCollide dan isPush bernilai true dan box belum dipindahkan
        if (isBoxCollide && isPush && !hasMovedBox)
        {
            MoveBox();
            hasMovedBox = true;  // Set menjadi true agar box tidak dipindahkan berulang kali
            transform.SetParent(null); // Pastikan player tidak menjadi child dari object apapun
            StartCoroutine(ResetIsPush()); // Mulai coroutine untuk reset isPush setelah 0.5 detik
        }

        // Memindahkan box melalui boxScript jika sudah diatur
        if (boxScript != null)
        {
            try
            {
                boxScript.MoveBox(targetPosBox);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        // Cek apakah player sedang bergerak
        if (isBoxCollide && !isPush && hasMovedBox)
        {
            // Jika player bergerak, terus mendorong box
            if (moveInput != Vector2.zero)
            {
                isPush = true; // Aktifkan mendorong kembali
                StartCoroutine(ResetIsPush()); // Mulai coroutine untuk reset isPush
            }
        }

        boxDirTarget = CreateRaycast(.5f, pushLayer);
    }

    private void FixedUpdate()
    {
        PlayerLogic(rb);
    }

    private void PlayerLogic(Rigidbody rb)
{
    // Mendapatkan referensi ke CinemachineBrain untuk menentukan kamera aktif
    CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    Transform cameraTransform;

    // Cek apakah ada CinemachineBrain yang mengelola kamera aktif
    if (cinemachineBrain != null && cinemachineBrain.ActiveVirtualCamera != null)
    {
        // Jika Cinemachine digunakan, ambil transform kamera virtual yang aktif
        cameraTransform = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform;
    }
    else
    {
        // Fallback ke kamera utama jika Cinemachine tidak digunakan
        cameraTransform = Camera.main.transform;
    }

    // Menghitung arah kamera pada sumbu horizontal (agar tidak terpengaruh oleh kemiringan vertikal kamera)
    Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
    Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

    // Menghitung arah player berdasarkan input dan arah kamera
    playerDirection = (cameraRight * moveInput.x + cameraForward * moveInput.y).normalized;

    // Mengatur kecepatan gerakan player
    Vector3 moveVelocity = playerDirection * moveSpeed;
    rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
}


    private void MoveBox()
    {
        // Tentukan arah pergerakan box berdasarkan boxPushTargetDir
        if (boxScript.boxDir == Vector3.forward)
        {
            if(boxScript.canPush) {
                targetPosBox += new Vector3(0f, 0f, boxScript.gridSize.z);
            }
        }
        else if (boxScript.boxDir == Vector3.back)
        {
            if(boxScript.canPush) {
                targetPosBox += new Vector3(0f, 0f, -boxScript.gridSize.z);
            }
        }
        else if (boxScript.boxDir == Vector3.right)
        {
            if(boxScript.canPush) {
                targetPosBox += new Vector3(boxScript.gridSize.x, 0f, 0f);
            }
        }
        else if (boxScript.boxDir == Vector3.left)
        {
            if(boxScript.canPush) {
                targetPosBox += new Vector3(-boxScript.gridSize.x, 0f, 0f);
            }
        }
    }

    // Coroutine untuk mengatur isPush menjadi false setelah 0.5 detik dan mengulangi
    private IEnumerator ResetIsPush()
    {
        rb.transform.SetParent(boxRb.transform);
        yield return new WaitForSeconds(0.5f);
        isPush = false;
        hasMovedBox = false; // Reset agar pergerakan bisa diulangi
        rb.transform.SetParent(null);
        yield return new WaitForSeconds(0.5f);
        
        // Cek apakah player masih bergerak, jika iya, aktifkan isPush
        if (playerDirection == boxScript.boxDir)
        {
            isPush = true; // Setelah 0.5 detik, set isPush kembali ke true
            StartCoroutine(ResetIsPush()); // Mulai ulang coroutine jika masih bergerak
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxCollide = true;

            if (!delayRunning) // Jalankan delay hanya jika belum berjalan
            {
                StartCoroutine(HandlePushDelay());
            }

            boxRb = collision.rigidbody;
            boxScript = boxRb.GetComponent<NewBoxScript>();
            targetPosBox = boxRb.transform.position;
            // SetPos(boxRb.transform);
            Debug.Log("IsPush: " + isPush);
        }
    }

    private IEnumerator HandlePushDelay()
    {
        delayRunning = true;
        yield return new WaitForSeconds(0.5f);
        isPush = true;
        delayRunning = false; 
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxCollide = false;
            isPush = false; 
            // boxDirTarget = Vector3.zero;
            hasMovedBox = false; // Reset hasMovedBox ketika player keluar dari collision
            Debug.Log("IsPush: " + isPush);
        }
    }

    private Vector3 CreateRaycast(float rayLength, LayerMask boxLayer) {
        Ray ray = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.forward);
        Ray ray1 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.back);
        Ray ray2 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.right);
        Ray ray3 = new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.left);

        if(Physics.Raycast(ray, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitForward");
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.red);
            return ray.direction;
        }
        else if(Physics.Raycast(ray1, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitBack");
            Debug.DrawRay(ray1.origin, ray.direction * rayLength, color: Color.red);
            return ray1.direction;
        }
        else if(Physics.Raycast(ray2, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitRight");
            Debug.DrawRay(ray2.origin, ray.direction * rayLength, color: Color.red);
            return ray2.direction;
        }
        else if(Physics.Raycast(ray3, out hitInfo, rayLength, boxLayer, QueryTriggerInteraction.UseGlobal)) {
            Debug.Log("RaycastHitLeft");
            Debug.DrawRay(ray3.origin, ray.direction * rayLength, color: Color.red);
            return ray3.direction;
        }
        else {
            Debug.Log("Raycast Null");
            Debug.DrawRay(ray.origin, ray.direction * rayLength, color: Color.blue);
            Debug.DrawRay(ray1.origin, ray1.direction * rayLength, color: Color.blue);
            Debug.DrawRay(ray2.origin, ray2.direction * rayLength, color: Color.blue);
            Debug.DrawRay(ray3.origin, ray3.direction * rayLength, color: Color.blue);
            return Vector3.zero;
        }
    }
}
