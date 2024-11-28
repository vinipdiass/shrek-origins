using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{
    void Start(){
        ConfigurarBotao();
    }
    public Button retornarButton;
    void ConfigurarBotao()
    {
        retornarButton.onClick.AddListener(() => Retornar());
    }
    public void Retornar()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Start");
    }
}


