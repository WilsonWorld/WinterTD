using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string FirstLevel;
    public GameObject OptionsScreen;

    public void StartGame()
    {
        SceneManager.LoadScene(FirstLevel);
    }

    public void OpenOptions()
    {
        OptionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
