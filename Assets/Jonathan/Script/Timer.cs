using UnityEngine;
using UnityEngine.UI; // Jika menggunakan UI, bisa ditambahkan
using TMPro; // Pastikan TextMeshPro diimpor

public class Timer : MonoBehaviour
{
    // Waktu dalam detik
    public float timeLimit = 60f; // Ubah sesuai kebutuhan
    private float timeRemaining;

    // Referensi ke TextMeshPro untuk menampilkan timer
    public TextMeshProUGUI timerText;

    // Event yang akan dipanggil ketika waktu habis
    public delegate void TimerFinished();
    public event TimerFinished OnTimerFinished;

    private void Start()
    {
        // Inisialisasi timer
        timeRemaining = timeLimit;
        UpdateTimerText();
    }

    private void Update()
    {
        // Kurangi waktu yang tersisa
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();

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
    }
}
