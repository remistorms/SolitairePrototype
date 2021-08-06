using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScreen : UIScreen
{
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
}
