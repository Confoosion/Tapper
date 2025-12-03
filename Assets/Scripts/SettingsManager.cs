using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Singleton;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle vibrateToggle;
    private bool hasVibrations = true;
    private bool confirmed = false;

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

        // HAPTICS CHECK
        if(PlayerPrefs.HasKey("HasVibrate"))
        {
            bool vibrations = PlayerPrefs.GetInt("HasVibrate") == 0;
            vibrateToggle.isOn = vibrations;
            ToggleVibrations(vibrations);
        }
        else
        {
            PlayerPrefs.SetInt("HasVibrate", 1);
        }
    }

    public void ToggleSFXVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("SFX_Volume", 0f);
            PlayerPrefs.SetInt("IsSFXMuted", 0);
        }
        else
        {
            audioMixer.SetFloat("SFX_Volume", -80f);
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
        if(toggle)
            PlayerPrefs.SetInt("HasVibrate", 0);
        else
            PlayerPrefs.SetInt("HasVibrate", 1);

        hasVibrations = toggle;
        UIManager.Singleton.UpdateToggle(UIManager.Singleton.VibrationToggle, toggle);
    }

    public bool CheckVibrations()
    {
        return(hasVibrations);
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
        // LeaderboardManager.Singleton.RemoveLeaderboardEntry();
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("SavedHighScore");
        PlayerPrefs.DeleteKey("SavedGems");
        // UIManager.Singleton.UpdateNameInputField("Tapper");
        // UIManager.Singleton.UpdateHighscoreUI();
        UIManager.Singleton.UpdateLeafUI();
        PlayerPrefs.Save();
    }

    public void ClearConfirm()
    {
        confirmed = false;
        UIManager.Singleton.ShowConfirm(false);
    }

    // For input field
    // public void CheckUsernameValidity(string name)
    // {
    //     if (name.Length < 3)
    //     {
    //         // Name too short
    //         UIManager.Singleton.UpdateNameErrorText("Username is too short");
    //     }
    //     else
    //     {
    //         UIManager.Singleton.UpdateNameErrorText("");
    //     }
    // }

    // For final check before leaving the Settings screen
    // public bool IsUserValid(string user)
    // {
    //     if (string.IsNullOrEmpty(user) && name.Length < 3)
    //     {
    //         // Name too short
    //         UIManager.Singleton.UpdateNameErrorText("Username is too short");
    //         return (false);
    //     }
    //     else
    //     {
    //         UIManager.Singleton.UpdateNameErrorText("");
    //         return (true);
    //     }
    // }

    public void BackOutOfLeaderboard(GameObject screen)
    {
        // if (IsUserValid(UIManager.Singleton.GetNameInputField()))
        // {
        //     if (PlayerPrefs.GetString("Username", "Tapper") != UIManager.Singleton.GetNameInputField())
        //     {
        //         // LeaderboardManager.Singleton.SetUsername(UIManager.Singleton.GetNameInputField());
        //     }
        //     ScreenManager.Singleton.SwitchScreen(screen);
        // }
        ScreenManager.Singleton.SwitchScreen(screen);
    }
}
