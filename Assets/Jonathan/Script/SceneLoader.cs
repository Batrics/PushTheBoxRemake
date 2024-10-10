using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
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
}
