using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicOnTrigger : OnTriggerBase
{
    [SerializeField] AudioClip musicClip;

    private bool triedToChangeMusic = false;
    private bool musicChanged = false;

    private void Update()
    {
        if(triedToChangeMusic)
        {
            if (!musicChanged)
            {
                Trigger();
            }
        }
    }

    public override void Trigger()
    {
        if(MusicManager.Instance != null)
        {
            triedToChangeMusic = true;
            musicChanged = MusicManager.Instance.TryToSetMusicClip(musicClip);
        }
    }
}
