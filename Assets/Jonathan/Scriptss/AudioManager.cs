using UnityEngine;
using UnityEngine.Audio;
using TMPro; // Pastikan TextMeshPro diimpor

public class AudioManager : MonoBehaviour
{
    // Reference to the Audio Mixer
    public AudioMixer audioMixer;

    // Exposed parameter names in the Audio Mixer
    private const string MUSIC_VOLUME = "MUSIC";
    private const string SFX_VOLUME = "SFX";

    // Audio sources for music and SFX
    public AudioSource musicSource;
    public AudioSource sfxSource;

    // Audio clips for music and SFX
    public AudioClip[] musicClip;
    public AudioClip[] sfxClip;

    // Mute states
    private bool isMusicMuted = false;
    private bool isSFXMuted = false;

    // TextMeshPro for displaying the status of Music and SFX
    public TextMeshProUGUI musicStatusText;
    public TextMeshProUGUI sfxStatusText;

    private void Start()
    {
        // Load saved settings from PlayerPrefs
        LoadAudioSettings();

        // Update UI Texts if TextMeshPro references are set
        UpdateMusicStatusText();
        UpdateSFXStatusText();
    }

    /// <summary>
    /// Set the volume of Music and save it.
    /// </summary>
    /// <param name="volume">The volume level (range typically between -80 to 0 dB).</param>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);  // Save music volume
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set the volume of SFX and save it.
    /// </summary>
    /// <param name="volume">The volume level (range typically between -80 to 0 dB).</param>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);  // Save SFX volume
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Mute or unmute Music and save the state.
    /// </summary>
    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        float volume = isMusicMuted ? -80f : PlayerPrefs.GetFloat(MUSIC_VOLUME, 0f);
        audioMixer.SetFloat(MUSIC_VOLUME, volume);

        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);  // Save mute state
        PlayerPrefs.Save();

        // Update the TextMeshPro text for Music status
        UpdateMusicStatusText();
    }

    /// <summary>
    /// Mute or unmute SFX and save the state.
    /// </summary>
    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        float volume = isSFXMuted ? -80f : PlayerPrefs.GetFloat(SFX_VOLUME, 0f);
        audioMixer.SetFloat(SFX_VOLUME, volume);

        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);  // Save mute state
        PlayerPrefs.Save();

        // Update the TextMeshPro text for SFX status
        UpdateSFXStatusText();
    }

    /// <summary>
    /// Play the music clip.
    /// </summary>
    public void PlayMusic(int angka)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip[angka];
            musicSource.Play();
        }
    }

    /// <summary>
    /// Play the SFX clip.
    /// </summary>
    public void PlaySFX(int angka)
    {
        if (sfxSource != null && sfxClip != null)
        {
            sfxSource.clip = sfxClip[angka];
            sfxSource.Play();
        }
    }

    /// <summary>
    /// Stop the music.
    /// </summary>
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    /// <summary>
    /// Stop the SFX.
    /// </summary>
    public void StopSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
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

    /// <summary>
    /// Load audio settings from PlayerPrefs.
    /// </summary>
    private void LoadAudioSettings()
    {
        // Load saved music volume
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            float savedMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
            audioMixer.SetFloat(MUSIC_VOLUME, savedMusicVolume);
        }

        // Load saved SFX volume
        if (PlayerPrefs.HasKey(SFX_VOLUME))
        {
            float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME);
            audioMixer.SetFloat(SFX_VOLUME, savedSFXVolume);
        }

        // Load saved music mute state
        if (PlayerPrefs.HasKey("MusicMuted"))
        {
            isMusicMuted = PlayerPrefs.GetInt("MusicMuted") == 1;
            audioMixer.SetFloat(MUSIC_VOLUME, isMusicMuted ? -80f : PlayerPrefs.GetFloat(MUSIC_VOLUME, 0f));
        }

        // Load saved SFX mute state
        if (PlayerPrefs.HasKey("SFXMuted"))
        {
            isSFXMuted = PlayerPrefs.GetInt("SFXMuted") == 1;
            audioMixer.SetFloat(SFX_VOLUME, isSFXMuted ? -80f : PlayerPrefs.GetFloat(SFX_VOLUME, 0f));
        }
    }

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exiting the game..."); // Optional: log a message for debugging
        Application.Quit();

        // If running in the editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
        #endif
    }
}
