using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public Vector2 moveInput;
    public Vector3 moveDirection;
    public Vector3 playerDirection;
    public Vector3 moveDirectionRelativeToCamera;
    public GameManager gameManager;
    private Rigidbody rb;

    public float moveSpeed = 5f;        // Kecepatan gerakan player
    private bool isRestart = false;
    private bool pushAnim = false;
    private MoveBox moveBox;


    public Camera mainCamera; // Referensi ke kamera utama
    public Transform cameraTransform;

    private void Start()
    {
        moveBox = GetComponent<MoveBox>();
        // Membuat GameObject baru untuk menyimpan transformasi
        GameObject camDummyObject = new GameObject("DummyTransform");
        cameraTransform = camDummyObject.transform;

        // Mengkopi nilai posisi, rotasi, dan skala dari objek asli ke dummy
        cameraTransform.position = mainCamera.transform.position;
        cameraTransform.rotation = mainCamera.transform.rotation;
        cameraTransform.localScale = mainCamera.transform.localScale;

        cameraTransform.eulerAngles = new Vector3(0f, cameraTransform.eulerAngles.y, 0f);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Membekukan rotasi agar tidak terpengaruh oleh fisika
        // playerAnimator = GetComponent<Animator>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Mengambil kamera utama jika tidak diatur
        }

    }

    private void Update()
    {
        if(isRestart) {
            gameManager.Restart();
        }
        MovePlayer();
        Animation();
    }

    void MovePlayer()
    {
        // Ambil arah gerakan berdasarkan input player dan normalisasi untuk memastikan magnitudo tetap
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        playerDirection = new Vector3(Mathf.Round(moveDirection.x), 0, Mathf.Round(moveDirection.z));

        // Cek apakah animasi player bukan dalam kondisi push
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash != pushStateHash)
        {
            // Tentukan arah kamera (forward dan right) dan atur sumbu y menjadi 0 untuk menghindari pergerakan vertikal
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Normalisasi arah kamera agar konsisten
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Hitung arah pergerakan relatif terhadap kamera
            if (playerDirection != Vector3.zero)
            {
                moveDirectionRelativeToCamera = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;
                // Set kecepatan langsung pada Rigidbody untuk kontrol kecepatan konstan
                rb.velocity = moveDirectionRelativeToCamera * moveSpeed;
            }
            else
            {
                // Jika tidak ada input, set kecepatan ke nol agar berhenti
                rb.velocity = Vector3.zero;
                moveDirectionRelativeToCamera = Vector3.zero;
            }
        }
}



    //-------------------------------------------- Animation ---------------------------------------------------

    public Animator playerAnimator;
    public static int stateInfo;
    int pushStateHash = Animator.StringToHash("Push");

    private void Animation()
{
    AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0); 
    stateInfo = currentState.shortNameHash;
    
    // Periksa jika player bergerak
    if(moveInput != Vector2.zero) 
    {
        playerAnimator.SetBool("Walk", true);

        // Cek arah gerakan untuk mengatur flip sprite
        if (moveInput.x > 0) // Player bergerak ke kanan
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Set flip normal
        }
        else if (moveInput.x < 0) // Player bergerak ke kiri
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip sprite ke kiri
        }
    }
    else 
    {
        playerAnimator.SetBool("Walk", false);
    }

    // Cek apakah player sedang dalam kondisi push
    playerAnimator.SetBool("Push", moveBox.isPush);
}


    //----------------------------------------------------------------------------- Input ------------------------------------------------------------------------------//
    public void OnMove(InputAction.CallbackContext context)
    {
        // Hanya menerima input gerakan jika animasi tidak dalam state "Push"
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash != pushStateHash)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        // Mengatur ulang status restart
        isRestart = context.performed;
    }
}
