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
        GameManager.Instance.StartGame();
    }

    public void OnThreeCardButtonPressed()
    {
        GameManager.Instance.StartGame(true);
    }

    public void OnUnshuffledButtonPressed()
    {
        GameManager.Instance.StartGame(true, false);
    }

    public void OnPauseMenuPressed()
    {
        GameManager.Instance.TogglePause();
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
