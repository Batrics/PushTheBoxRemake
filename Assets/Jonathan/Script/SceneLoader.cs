using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Variabel untuk perhitungan
    public int countValue = 0;
    public bool shouldCount = false; // Jika true, perhitungan akan dimulai
    public int targetCount = 30; // Target nilai yang harus dicapai
    public string targetSceneName; // Nama scene yang akan dimuat ketika countValue mencapai target

    /// <summary>
    /// Fungsi untuk memuat scene berdasarkan nama scene.
    /// </summary>
    /// <param name="sceneName">Nama scene yang ingin dimuat.</param>
    public void LoadSceneByName(string sceneName)
    {
        // Pastikan nama scene valid dan ada dalam build settings
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Memuat scene sesuai dengan nama yang diberikan
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1;
        }
        else
        {
            Debug.LogError("Nama scene tidak valid atau kosong.");
        }
    }

    public void LoadSceneByNameWithDelay(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Memulai coroutine untuk menunda pemuatan scene
            StartCoroutine(LoadSceneAfterDelay(sceneName, 5));
        }
        else
        {
            Debug.LogError("Nama scene tidak valid atau kosong.");
        }
    }
    
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        // Menunggu selama waktu yang ditentukan
        yield return new WaitForSeconds(delay);

        // Memuat scene setelah waktu tunda
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        // Jika bool shouldCount true, mulai perhitungan
        if (shouldCount)
        {
            StartCoroutine(CountAndLoadScene());
        }
    }

    private IEnumerator CountAndLoadScene()
    {
        while (countValue < targetCount)
        {
            // Menunggu selama 1 detik setiap kali melakukan perhitungan
            yield return new WaitForSeconds(1);
            countValue++;

            // Jika countValue mencapai target, load scene
            if (countValue >= targetCount)
            {
                LoadSceneByName(targetSceneName);
            }
        }
    }
}
