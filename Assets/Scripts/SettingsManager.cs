using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;

    void Start()
    {
        GetPlayerSettings();
    }

    void GetPlayerSettings()
    {
        bool isMuted;
        if (PlayerPrefs.HasKey("IsSFXMuted"))
        {
            isMuted = PlayerPrefs.GetInt("IsSFXMuted") == 0;
            sfxToggle.isOn = isMuted;
            ToggleSFXVolume(isMuted);
        }
        else
        {
            PlayerPrefs.SetInt("IsSFXMuted", 0);
        }

        if (PlayerPrefs.HasKey("IsMusicMuted"))
        {
            isMuted = PlayerPrefs.GetInt("IsMusicMuted") == 0;
            musicToggle.isOn = isMuted;
            ToggleMusicVolume(isMuted);
        }
        else
        {
            PlayerPrefs.SetInt("IsMusicMuted", 0);
        }
    }

    public void ToggleSFXVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("SFX_Volume", 0f);
            PlayerPrefs.SetInt("IsSFXMuted", 0);
            return;
        }
        // Debug.Log("SFX MUTED");
        audioMixer.SetFloat("SFX_Volume", -80f);
        PlayerPrefs.SetInt("IsSFXMuted", 1);
    }

    public void ToggleMusicVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("Music_Volume", -10f);
            PlayerPrefs.SetInt("IsMusicMuted", 0);
            return;
        }
        // Debug.Log("MUSIC MUTED");
        audioMixer.SetFloat("Music_Volume", -80f);
        PlayerPrefs.SetInt("IsMusicMuted", 1);
    }

    
}
