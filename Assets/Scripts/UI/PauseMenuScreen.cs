using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScreen : UIScreen
{
    private SoundManager soundManager;
    [SerializeField] private Slider m_musicSlider;
    [SerializeField] private Slider m_sfxSlider;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void OnOneCardButtonPressed()
    {
        GameManager.instance.StartGame();
    }

    public void OnThreeCardButtonPressed()
    {
        GameManager.instance.StartGame(true);
    }

    public void OnUnshuffledButtonPressed()
    {
        GameManager.instance.StartGame(true, false);
    }

    public void OnPauseMenuPressed()
    {
        GameManager.instance.TogglePause();
    }

    public void OnVolumeSliderChanged(float value)
    {
        soundManager.SetMusicVolume(m_musicSlider.value);
    }

    public void OnSFXSliderChanged(float value)
    {
        soundManager.SetSFXVolume(m_sfxSlider.value);
    }
}
