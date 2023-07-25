using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    [SerializeField] private float musicChangeBridgeMultiplier = 1f;
    [SerializeField] private float maxTimeUntilBackToNonCombatMusic = 5f;
    [SerializeField] private float maxTimeUntilBackToDefaultMusic = 5f;
    [SerializeField] private AudioClip[] combatAudioClips;
    [SerializeField] private AudioClip[] sneakingAudioClips;
    [SerializeField] private AudioClip[] defaultAudioClips;
    [SerializeField] private bool scriptedMusicHandling = false;


    private AudioSource audioSource;
    private AudioClip newAudioClip;
    private float volume = .1f;
    private float currentTimeUntilBackToNonCombatMusic = 0f;
    private float currentTimeUntilBackToDefaultMusic = 0f;

    private bool isChangingAudioClip = false;
    private bool increaseVolumeAfterChanging = false;
    public bool isInCombat = false;
    public bool isSneaking = false;
    public bool isPlayingCombatMusic = false;
    public bool isPlayingSneakingMusic = false;
    public bool isPlayingDefaultMusic = false;
    

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        if (!scriptedMusicHandling)
        {
            int defaultAudioClipIndex = Random.Range(0, defaultAudioClips.Length);
            AudioClip audioClip = defaultAudioClips[defaultAudioClipIndex];
            audioSource.clip = audioClip;
            audioSource.Play();
            isPlayingDefaultMusic = true;
            isPlayingSneakingMusic = false;
            isPlayingCombatMusic = false;
        }

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.2f);
        audioSource.volume = volume;
    }

    private void Update()
    {
        HandleMusicChange();
        if(isChangingAudioClip)
        {
            if(!increaseVolumeAfterChanging )
            {
                audioSource.volume -= Time.deltaTime * musicChangeBridgeMultiplier * volume;
                if(audioSource.volume <= 0f )
                {
                    audioSource.clip = newAudioClip;
                    audioSource.Play();
                    increaseVolumeAfterChanging = true;
                }
            }
            else
            {
                audioSource.volume += Time.deltaTime * musicChangeBridgeMultiplier * volume;
                if(audioSource.volume >= volume)
                {

                    audioSource.volume = volume;
                    isChangingAudioClip = false;
                    increaseVolumeAfterChanging = false;
                }
            }
        }
        currentTimeUntilBackToNonCombatMusic += Time.deltaTime;
        currentTimeUntilBackToDefaultMusic += Time.deltaTime;
        if (currentTimeUntilBackToNonCombatMusic >= maxTimeUntilBackToNonCombatMusic)
        {
            isInCombat = false;
        }
        if (currentTimeUntilBackToDefaultMusic >= maxTimeUntilBackToDefaultMusic)
        {
            isSneaking = false;
        }
    }

    private void HandleMusicChange()
    {
        if (!scriptedMusicHandling)
        {

            if (!isInCombat)
            {
                if (!isSneaking)
                {
                    if (!isPlayingDefaultMusic)
                    {
                        int defaultAudioClipIndex = Random.Range(0, defaultAudioClips.Length);
                        AudioClip audioClip = defaultAudioClips[defaultAudioClipIndex];
                        isPlayingDefaultMusic = TryToSetMusicClip(audioClip); ;
                        isPlayingSneakingMusic = false;
                        isPlayingCombatMusic = false;
                    }

                }
                else if (!isPlayingSneakingMusic)
                {
                    int sneakingAudioClipIndex = Random.Range(0, sneakingAudioClips.Length);
                    AudioClip audioClip = sneakingAudioClips[sneakingAudioClipIndex];
                    isPlayingSneakingMusic = TryToSetMusicClip(audioClip);
                    isPlayingCombatMusic = false;
                    isPlayingDefaultMusic = false;
                }
            }
            else
            {
                if (!isPlayingCombatMusic)
                {
                    int combatAudioClipIndex = Random.Range(0, combatAudioClips.Length);
                    AudioClip audioClip = combatAudioClips[combatAudioClipIndex];
                    isPlayingCombatMusic = TryToSetMusicClip(audioClip);
                    isPlayingSneakingMusic = false;
                    isPlayingDefaultMusic = false;
                }
            }
        }
        else
        {
            currentTimeUntilBackToNonCombatMusic = maxTimeUntilBackToNonCombatMusic;
            currentTimeUntilBackToDefaultMusic = maxTimeUntilBackToDefaultMusic;
            isPlayingCombatMusic = false;
            isPlayingSneakingMusic = false;
            isPlayingDefaultMusic = false;
        }
    }
    public void DecreaseVolume()
    {
        volume -= .1f;
        if (volume <= 0)
        {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void IncreaseVolume()
    {
        volume += .1f;
        if (volume >= 1f)
        {
            volume = 1f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

    public bool TryToSetMusicClip(AudioClip audioClip)
    {
        if (!isChangingAudioClip)
        {
            isChangingAudioClip = true;
            newAudioClip = audioClip;
            return true;
        }
        return false;
    }

    public void ActivateDefaultMusicHandling()
    {
        scriptedMusicHandling = false;
    }

    public void ActivateScriptedMusicHandling()
    {
        scriptedMusicHandling = true;
        isPlayingSneakingMusic = false;
        isPlayingCombatMusic = false;
    }

    public void CombatTrigger()
    {
        isSneaking = false;
        isInCombat = true;
        currentTimeUntilBackToNonCombatMusic = 0;
    }

    public void SneakTrigger()
    {
        if(!isInCombat)
        {
            isSneaking = true;
            currentTimeUntilBackToDefaultMusic = 0;
        }
    }
}
