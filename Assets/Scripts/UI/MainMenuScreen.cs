using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : UIScreen
{
    public void OnPlayButtonPressed()
    {
        GameManager.Instance.StartGame();
    }
}
