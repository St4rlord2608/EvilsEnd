using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicQuestLock : QuestLock
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private bool scriptedMusicEnabled = true;

    private bool triedToChangeMusic = false;
    private bool musicChanged = false;

    private void Update()
    {
        if (triedToChangeMusic)
        {
            if (!musicChanged)
            {
                QuestCompleted();
            }
        }
    }
    public override void QuestCompleted()
    {
        if (MusicManager.Instance != null)
        {
            if(scriptedMusicEnabled)
            {
                triedToChangeMusic = true;
                MusicManager.Instance.ActivateScriptedMusicHandling();
                musicChanged = MusicManager.Instance.TryToSetMusicClip(musicClip);
            }
            else
            {
                MusicManager.Instance.ActivateDefaultMusicHandling();
            }
        }
    }
}
