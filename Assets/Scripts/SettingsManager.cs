using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void ToggleSFXVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("SFX_Volume", 0f);
            return;
        }
        audioMixer.SetFloat("SFX_Volume", -80f);
    }

    public void ToggleMusicVolume(bool toggle)
    {
        if (toggle)
        {
            audioMixer.SetFloat("Music_Volume", -10f);
            return;
        }
        audioMixer.SetFloat("Music_Volume", -80f);
    }
}
