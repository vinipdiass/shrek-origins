using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar cenas
using UnityEngine.UI; // Necessário para manipular botões

public class ButtonLoadScene : MonoBehaviour
{
    public Button loadSceneButton; // Referência ao botão na UI
    public string targetScene = "Menu Start"; // Nome da cena a ser carregada

    void Start()
    {
        // Certifique-se de que o botão está configurado
        if (loadSceneButton != null)
        {
            // Adiciona o listener para o clique do botão
            loadSceneButton.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogError("O botão não foi atribuído no Inspector!");
        }
    }

    private void LoadScene()
    {
        // Carrega a cena especificada
        SceneManager.LoadScene(targetScene);
    }
}
