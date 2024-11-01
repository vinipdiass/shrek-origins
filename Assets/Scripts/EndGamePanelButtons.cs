using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePanelButtons : MonoBehaviour
{
    public void ButtonPlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
    }

    public void ButtonReturnMenuTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Start");
    }

    public void ButtonQuitGame()
    {
        Application.Quit();
    }
}
