using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private float volume = 1f;

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public event EventHandler OnSoundEffectVolumeChange;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }

    public void DecreaseVolume()
    {
        volume -= .1f;
        if (volume <= 0)
        {
            volume = 0f;
        }
        OnSoundEffectVolumeChange?.Invoke(this, EventArgs.Empty);

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void IncreaseVolume()
    {
        volume += .1f;
        if (volume >= 1f)
        {
            volume = 1f;
        }
        OnSoundEffectVolumeChange?.Invoke(this, EventArgs.Empty);
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
