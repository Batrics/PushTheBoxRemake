using UnityEngine;
using TMPro; // Pastikan TextMeshPro diimpor

public class PauseMenu : MonoBehaviour
{
    // Referensi untuk UI Pause dan TextMeshPro
    public GameObject pauseMenuUI;
    public TextMeshProUGUI pauseText;

    // Status pause
    private bool isPaused = false;

    private void Update()
    {
        // Cek jika tombol Esc ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Mengaktifkan UI Pause
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 1f; // Hentikan waktu
        isPaused = true;
        UpdatePauseText();
    }

    // Menyembunyikan UI Pause
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Kembalikan waktu
        isPaused = false;
        UpdatePauseText();
    }

    // Mengubah teks berdasarkan status pause
    private void UpdatePauseText()
    {
        if (pauseText != null)
        {
            pauseText.text = isPaused ? "Continue" : "Pause";
        }
    }
}
