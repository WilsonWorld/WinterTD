using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenu : MonoBehaviour
{
    public void OnDefeatRestartClick()
    {
        LevelManager.Instance.RestartLevel();
        LevelManager.Instance.CloseDefeatScreen();
    }

    public void OnVictoryRestartClick()
    {
        LevelManager.Instance.RestartLevel();
        LevelManager.Instance.CloseVictoryScreen();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
