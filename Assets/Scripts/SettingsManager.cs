using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Singleton;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibrationToggle;
    private bool hasVibrations = true;
    // private bool confirmed = false;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }
    }

    void Start()
    {
        GetPlayerSettings();
    }

    void GetPlayerSettings()
    {
        bool isMuted;
        bool noVibrations;

        // SFX CHECK
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

        // MUSIC CHECK
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

        // HAPTIC CHECK
        if(PlayerPrefs.HasKey("HasVibrations"))
        {
            noVibrations = PlayerPrefs.GetInt("HasVibrations") == 0;
            vibrationToggle.isOn = noVibrations;
            ToggleVibrations(noVibrations);
        }
        else
        {
            PlayerPrefs.SetInt("HasVibrations", 0);
        }
    }

    public void ToggleSFXVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("SFX_Volume", 0f);
            audioMixer.SetFloat("Tap_Volume", 0f);
            PlayerPrefs.SetInt("IsSFXMuted", 0);
        }
        else
        {
            audioMixer.SetFloat("SFX_Volume", -80f);
            audioMixer.SetFloat("Tap_Volume", -80f);
            PlayerPrefs.SetInt("IsSFXMuted", 1);
        }

        UIManager.Singleton.UpdateToggle(UIManager.Singleton.SFXToggle, toggle);
    }

    public void ToggleMusicVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("Music_Volume", -10f);
            PlayerPrefs.SetInt("IsMusicMuted", 0);
        }
        else
        {
            audioMixer.SetFloat("Music_Volume", -80f);
            PlayerPrefs.SetInt("IsMusicMuted", 1);
        }

        UIManager.Singleton.UpdateToggle(UIManager.Singleton.MusicToggle, toggle);
    }

    public void ToggleVibrations(bool toggle)
    {
        if (toggle)
        {
            PlayerPrefs.SetInt("HasVibrations", 0);
            hasVibrations = false;
        }
        else
        {
            PlayerPrefs.SetInt("HasVibrations", 1);
            hasVibrations = true;
        }

        UIManager.Singleton.UpdateToggle(UIManager.Singleton.VibrationToggle, !toggle);
    }

    public bool CheckVibrations()
    {
        return(hasVibrations);
    }
}
