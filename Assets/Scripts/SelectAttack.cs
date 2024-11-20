using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAttack : MonoBehaviour
{
    public void ButtonPunch()
    {
        loadMap();
        PlayerPrefs.SetString("PlayerAbility", "Punch");
    }

    public void ButtonRoar()
    {
        loadMap();
        PlayerPrefs.SetString("PlayerAbility", "Roar");

    }

    public void ButtonFart()
    {
        loadMap();
        PlayerPrefs.SetString("PlayerAbility", "Fart");

    }

    public void ButtonOnion()
    {
        loadMap();
        PlayerPrefs.SetString("PlayerAbility", "GasAttack");

    }

    public void ButtonBeetle()
    {
        loadMap();
        PlayerPrefs.SetString("PlayerAbility", "BeetleAttack");

    }

    public void ButtonFrog()
    {
        loadMap();
        PlayerPrefs.SetString("PlayerAbility", "BoomerangAttack");

    }

    public void loadMap(){
        string playerAbility = PlayerPrefs.GetString("Map", "None");
        if (playerAbility == "Map1")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        }
        else if (playerAbility == "Map2")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        }
        else if (playerAbility == "Map3")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        }
        else if (playerAbility == "Map4")
        {
             UnityEngine.SceneManagement.SceneManager.LoadScene("Fase 1");
        }
    }


}
