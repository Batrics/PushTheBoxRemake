using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneLoader : MonoBehaviour
{
    // Nama scene yang akan dimuat
    public string sceneToLoad = "Clossing";
    // Delay sebelum scene dimuat (dalam detik)
    public float delayBeforeLoading = 6f;

    // Fungsi yang dipanggil ketika ada trigger collision
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek yang ter-trigger memiliki tag "Player"
        if (other.CompareTag("Player"))
        {
            // Mulai coroutine untuk delay sebelum load scene
            StartCoroutine(LoadSceneWithDelay());
        }
    }

    // Coroutine untuk delay sebelum memuat scene
    private IEnumerator LoadSceneWithDelay()
    {
        // Tunggu selama delayBeforeLoading detik
        yield return new WaitForSeconds(delayBeforeLoading);

        // Memuat scene dengan nama yang ditentukan
        SceneManager.LoadScene(sceneToLoad);
    }
}
