using UnityEngine;
using TMPro; // Pastikan TextMeshPro diimpor

public class TextToggleManager : MonoBehaviour
{
    // Reference to TextMeshPro UI components for Music and SFX
    public TextMeshProUGUI musicStatusText;
    public TextMeshProUGUI sfxStatusText;

    // Reference to AudioManager (script sebelumnya)
    public AudioManager audioManager;

    private bool isMusicMuted;
    private bool isSFXMuted;

    private void Start()
    {
        // Load initial states from AudioManager or PlayerPrefs
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        // Update the UI text on Start
        UpdateMusicStatusText();
        UpdateSFXStatusText();
    }

    /// <summary>
    /// Unified function to toggle Music and update text
    /// </summary>
    public void ToggleMusicAndUpdateText()
    {
        // Toggle music mute
        isMusicMuted = !isMusicMuted;
        audioManager.ToggleMusicMute();

        // Save the music state
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);

        // Update the TextMeshPro text for Music
        UpdateMusicStatusText();
    }

    /// <summary>
    /// Unified function to toggle SFX and update text
    /// </summary>
    public void ToggleSFXAndUpdateText()
    {
        // Toggle SFX mute
        isSFXMuted = !isSFXMuted;
        audioManager.ToggleSFXMute();

        // Save the SFX state
        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);

        // Update the TextMeshPro text for SFX
        UpdateSFXStatusText();
    }

    /// <summary>
    /// Update the TextMeshPro text based on whether the music is muted.
    /// </summary>
    private void UpdateMusicStatusText()
    {
        if (musicStatusText != null)
        {
            // Update text based on whether music is muted
            musicStatusText.text = isMusicMuted ? "Off" : "On";
        }
    }

    /// <summary>
    /// Update the TextMeshPro text based on whether the SFX is muted.
    /// </summary>
    private void UpdateSFXStatusText()
    {
        if (sfxStatusText != null)
        {
            // Update text based on whether SFX is muted
            sfxStatusText.text = isSFXMuted ? "Off" : "On";
        }
    }
}
