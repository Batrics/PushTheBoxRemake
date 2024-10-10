using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Pastikan DoTween diimpor
using System.Collections;

public class AlertBlink : MonoBehaviour
{
    // Referensi ke UI Image yang akan berkedip
    public Image alertImage;
    public float blinkDuration = 0.5f;  // Durasi satu kedipan (alpha 0 ke 0.1 atau sebaliknya)
    public float timeAtAlphaMax = 0.5f; // Waktu yang dihabiskan saat alpha 0.1 (kedip maksimal)

    // Referensi ke AudioManager untuk memainkan SFX
    public AudioManager audioManager;
    public int sfxIndex = 0;            // Indeks dari SFX yang ingin diputar

    private bool isBlinking = false;

    private void Start()
    {
        // Memastikan alpha Image mulai dari 0
        SetImageAlpha(0);
    }

    /// <summary>
    /// Fungsi untuk memulai blink dan memainkan SFX
    /// </summary>
    public void StartBlink()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            StartCoroutine(BlinkRoutine());

            // Memutar SFX secara looping
            audioManager.PlaySFX(sfxIndex);
            audioManager.sfxSource.loop = false; // Loop SFX
        }
    }

    /// <summary>
    /// Fungsi untuk menghentikan blink dan menghentikan SFX setelah 1 detik
    /// </summary>
    public void StopBlink()
    {
        if (isBlinking)
        {
            isBlinking = false;
            StopCoroutine(BlinkRoutine());

            // Hentikan SFX setelah 1 detik
            StartCoroutine(StopSFXAfterDelay(1f));

            // Setel alpha kembali ke 0
            SetImageAlpha(0);
        }
    }

    // Coroutine untuk mengatur kedipan dari alpha 0 ke 0.1, lalu kembali ke 0
    private IEnumerator BlinkRoutine()
    {
        while (isBlinking)
        {
            // Fade in (alpha 0 ke 0.1)
            alertImage.DOFade(0.1f, blinkDuration);

            // Tunggu di alpha 0.1 selama timeAtAlphaMax
            yield return new WaitForSeconds(timeAtAlphaMax);

            // Fade out (alpha 0.1 ke 0)
            alertImage.DOFade(0, blinkDuration);

            // Tunggu sebelum memulai kedipan berikutnya
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    /// <summary>
    /// Coroutine untuk menghentikan SFX setelah delay
    /// </summary>
    private IEnumerator StopSFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Menghentikan SFX setelah 1 detik
        audioManager.sfxSource.loop = false; // Hentikan loop
        audioManager.StopSFX();
    }

    /// <summary>
    /// Fungsi untuk mengatur alpha dari Image UI
    /// </summary>
    /// <param name="alpha">Nilai alpha (0-1)</param>
    private void SetImageAlpha(float alpha)
    {
        Color tempColor = alertImage.color;
        tempColor.a = alpha;
        alertImage.color = tempColor;
    }
}
