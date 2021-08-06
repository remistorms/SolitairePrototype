using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;

    public void SetMusicVolume(float volume)
    {
        m_audioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        EventsManager.Fire_evt_SFXVolumeChanged(volume);
    }
}
