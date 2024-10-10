using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Waktu dalam detik
    public float timeLimit = 600f; // Ubah sesuai kebutuhan
    private float timeRemaining;

    // Referensi ke TextMeshPro untuk menampilkan timer
    public TextMeshProUGUI timerText;

    // Event yang akan dipanggil ketika waktu habis
    public delegate void TimerFinished();
    public event TimerFinished OnTimerFinished;

    // Referensi ke SaveManager untuk menyimpan waktu
    private SaveManager saveManagerTimer;

    // Referensi ke AlertBlink untuk memulai kedipan
    public AlertBlink alertBlink;
    private bool alertStarted = false; // Untuk memastikan alert hanya dipanggil sekali

    // GameObject yang akan diaktifkan ketika waktu habis
    public GameObject objectToActivate;
    
    // GameObject kedua yang akan diaktifkan 1 detik sebelum game di-pause
    public GameObject secondObjectToActivate;

    // Waktu delay sebelum game di-pause setelah GameObject diaktifkan
    public float pauseDelay = 8f;

    private void Start()
    {
        saveManagerTimer = GetComponent<SaveManager>();

        if (PlayerPrefs.HasKey(saveManagerTimer.name))
        {
            timeRemaining = PlayerPrefs.GetFloat(saveManagerTimer.name);
            UpdateTimerText();
        }
        else
        {
            timeRemaining = timeLimit;
            UpdateTimerText();
        }
    }

    private void Update()
    {
        // Kurangi waktu yang tersisa
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();

            // Cek jika waktu tersisa 5 menit dan belum memulai alert
            if (timeRemaining <= 300 && !alertStarted) // 300 detik = 5 menit
            {
                alertBlink.StartBlink();
                alertStarted = true; // Pastikan alert hanya dipanggil sekali
            }

            // Cek jika waktu habis
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                TimerEnd();
            }
        }
    }

    // Memperbarui tampilan timer
    private void UpdateTimerText()
    {
        // Menghitung menit dan detik
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Fungsi yang dipanggil ketika waktu habis
    private void TimerEnd()
    {
        // Panggil event
        OnTimerFinished?.Invoke();

        // Aktifkan GameObject pertama ketika waktu habis
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            alertBlink.StopBlink();
            
            // Aktifkan GameObject kedua 1 detik sebelum game dijeda
            Invoke("ActivateSecondObject", pauseDelay - 1f);

            // Jalankan coroutine untuk menunda pause game setelah pauseDelay
            Invoke("PauseGame", pauseDelay);
        }
    }

    // Fungsi untuk mengaktifkan GameObject kedua
    private void ActivateSecondObject()
    {
        if (secondObjectToActivate != null)
        {
            secondObjectToActivate.SetActive(true);
        }
    }

    // Fungsi untuk menjeda game
    private void PauseGame()
    {
        // Pause game dengan mengatur timeScale ke 0
        Time.timeScale = 0f;
    }

    public void SavingTimer()
    {
        saveManagerTimer.SavingFloat(timeRemaining);
    }
}
