using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void ButtonNewGame()
    {
        Debug.Log("New Game");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select Attack");
    }

    public void ButtonOptions()
    {
        Debug.Log("Options");
    }

    public void ButtonCredits()
    {
        Debug.Log("Credits");
    }

    public void ButtonQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
