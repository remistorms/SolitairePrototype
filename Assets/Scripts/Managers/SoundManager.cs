using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] AudioClip[] availableClips;

    private void Start()
    {
        ChangeMusic();
    }

    public void ChangeMusic()
    {
        SelectMusic(Random.Range(0, availableClips.Length));

        m_audioSource.Play();
    }

    public void SelectMusic(int clipIndex)
    {
        m_audioSource.clip = availableClips[clipIndex];
    }

    public void SetMusicVolume(float volume)
    {
        m_audioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        EventsManager.Fire_evt_SFXVolumeChanged(volume);
    }
}
