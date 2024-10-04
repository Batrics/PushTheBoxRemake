using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

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
    private BoxScript boxScript;

    public float moveSpeed = 5f;
    public float rotationSpeed = 2;
    public float lookSensitivity = 1f;
    public float X;
    public float Y;
    public float Z;

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
                boxScript.MoveBox(targetPosBox, gameObject);
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
    }

    private void FixedUpdate()
    {
        PlayerLogic(rb);
    }

    private void PlayerLogic(Rigidbody rb)
    {
        playerDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 moveVelocity = playerDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void MoveBox()
    {
        // Tentukan arah pergerakan box berdasarkan boxPushTargetDir
        if (boxScript.boxPushTargetDir == Vector3.forward)
        {
            targetPosBox += new Vector3(0f, 0f, boxScript.gridSize.z);
        }
        else if (boxScript.boxPushTargetDir == Vector3.back)
        {
            targetPosBox += new Vector3(0f, 0f, -boxScript.gridSize.z);
        }
        else if (boxScript.boxPushTargetDir == Vector3.right)
        {
            targetPosBox += new Vector3(boxScript.gridSize.x, 0f, 0f);
        }
        else if (boxScript.boxPushTargetDir == Vector3.left)
        {
            targetPosBox += new Vector3(-boxScript.gridSize.x, 0f, 0f);
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
        if (playerDirection == boxScript.boxPushTargetDir)
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
            boxScript = boxRb.GetComponent<BoxScript>();
            targetPosBox = boxRb.transform.position;
            SetPos(boxRb.transform);
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

    private void SetPos(Transform transform)
    {
        X = transform.position.x;
        Y = transform.position.y;
        Z = transform.position.z;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxCollide = false;
            isPush = false; 
            boxDirTarget = Vector3.zero;
            hasMovedBox = false; // Reset hasMovedBox ketika player keluar dari collision
            Debug.Log("IsPush: " + isPush);
        }
    }
}
