using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] AudioClip[] availableClips;

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (m_audioSource.clip == null)
        {
            SelectMusic(Random.Range(0, availableClips.Length));
        }

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
