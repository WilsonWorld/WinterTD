using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] LevelManager m_LevelManager;

    public void OnRestartClick()
    {
        m_LevelManager.RestartLevel();
        m_LevelManager.CloseDefeatScreen();
    }
}
