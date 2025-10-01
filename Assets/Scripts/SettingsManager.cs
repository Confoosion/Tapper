using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    private bool confirmed = false;

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

    public void ResetDataPressed()
    {
        if (confirmed)
        {
            UIManager.Singleton.UpdateConfirm();
            ResetData();
            confirmed = false;
        }
        else
        {
            UIManager.Singleton.ShowConfirm(true);
            confirmed = true;
        }
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        LeaderboardManager.Singleton.RemoveLeaderboardEntry();
        UIManager.Singleton.UpdateHighscoreUI();
        UIManager.Singleton.UpdateGemUI();
        PlayerPrefs.Save();
    }

    public void ClearConfirm()
    {
        confirmed = false;
        UIManager.Singleton.ShowConfirm(false);
    }

    // For input field
    public void CheckUsernameValidity(string name)
    {
        if (name.Length < 3)
        {
            // Name too short
            UIManager.Singleton.UpdateNameErrorText("Username is too short");
        }
        else if (LeaderboardManager.Singleton.FindUsername(name))
        {
            // Not a unique username
            UIManager.Singleton.UpdateNameErrorText("Username is already taken");
        }
        else
        {
            UIManager.Singleton.UpdateNameErrorText("");
        }
    }

    // For final check before leaving the Settings screen
    public bool IsUserValid(string user)
    {
        if (string.IsNullOrEmpty(user) && name.Length < 3)
        {
            // Name too short
            UIManager.Singleton.UpdateNameErrorText("Username is too short");
            return (false);
        }
        else if (LeaderboardManager.Singleton.FindUsername(name))
        {
            // Not a unique username
            UIManager.Singleton.UpdateNameErrorText("Username is already taken");
            return (false);
        }
        else
        {
            UIManager.Singleton.UpdateNameErrorText("");
            return (true);
        }
    }

    public void BackOutOfSettings(ScreenSwapping screen)
    {
        if (IsUserValid(UIManager.Singleton.GetNameInputField()))
        {
            LeaderboardManager.Singleton.SetUsername(UIManager.Singleton.GetNameInputField());
            ScreenManager.Singleton.SwitchScreen(screen);
        }
    }
}
