using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAttack : MonoBehaviour
{
    public void ButtonPunch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        PlayerPrefs.SetString("PlayerAbility", "Punch");
    }

    public void ButtonRoar()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        PlayerPrefs.SetString("PlayerAbility", "Roar");

    }

    public void ButtonFart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        PlayerPrefs.SetString("PlayerAbility", "Fart");

    }

    public void ButtonOnion()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        PlayerPrefs.SetString("PlayerAbility", "GasAttack");

    }

    public void ButtonBeetle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        PlayerPrefs.SetString("PlayerAbility", "BeetleAttack");

    }

    public void ButtonFrog()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        PlayerPrefs.SetString("PlayerAbility", "BoomerangAttack");

    }


}
