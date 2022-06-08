using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string MainMenuLevel;
    public GameObject OptionsScreen;
    public AudioSource ButtonPressSFX;

    float m_Delay = 0.2f;

    IEnumerator StartDelayTimer()
    {
        yield return new WaitForSeconds(m_Delay);

        gameObject.SetActive(false);
    }

    IEnumerator QuitDelayTimer()
    {
        yield return new WaitForSeconds(m_Delay);

        SceneManager.LoadScene(MainMenuLevel);
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        StartCoroutine(StartDelayTimer());
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
        Time.timeScale = 1;
        StartCoroutine(QuitDelayTimer());
    }

    public void PlayButtonPressSFX()
    {
        if (ButtonPressSFX == null)
            return;

        ButtonPressSFX.PlayOneShot(ButtonPressSFX.clip);
    }
}
