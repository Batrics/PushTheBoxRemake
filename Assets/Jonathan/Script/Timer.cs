using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Waktu yang telah berlalu dalam detik
    private float elapsedTime = 0f;

    // Referensi ke TextMeshPro untuk menampilkan timer
    public TextMeshProUGUI timerText;

    // Referensi ke SaveManager untuk menyimpan waktu
    private SaveManager saveManagerTimer;

    // Referensi ke AlertBlink untuk memulai kedipan
    public AlertBlink alertBlink;
    private bool alertStarted = false;

    // GameObject yang akan diaktifkan berdasarkan kondisi lain, jika diperlukan
    public GameObject objectToActivate;

    // GameObject kedua yang akan diaktifkan
    public GameObject secondObjectToActivate;

    // Waktu delay sebelum game dijeda
    public float pauseDelay = 8f;

    private void Start()
    {
        saveManagerTimer = GetComponent<SaveManager>();

        if (PlayerPrefs.HasKey(saveManagerTimer.name))
        {
            elapsedTime = PlayerPrefs.GetFloat(saveManagerTimer.name);
            UpdateTimerText();
        }
        else
        {
            elapsedTime = 0f;
            UpdateTimerText();
        }
    }

    private void Update()
    {
        // Tingkatkan waktu yang telah berlalu
        elapsedTime += Time.deltaTime;
        UpdateTimerText();

        // Cek jika waktu telah mencapai 5 menit dan belum memulai alert
        if (elapsedTime >= 300f && !alertStarted) // 300 detik = 5 menit
        {
            alertBlink.StartBlink();
            alertStarted = true;
        }
    }

    // Memperbarui tampilan timer
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ActivateSecondObject()
    {
        if (secondObjectToActivate != null)
        {
            secondObjectToActivate.SetActive(true);
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void SavingTimer()
    {
        saveManagerTimer.SavingFloat(elapsedTime);
    }
}
