using UnityEngine;
using UnityEngine.UI; // Necessário para manipular UI
using UnityEngine.SceneManagement; // Necessário para carregar cenas
using System.Collections;

public class AppearImage : MonoBehaviour
{
    public float fadeDuration = 2f; // Duração do fade-in
    public Button sceneButton; // Referência ao botão na UI
    public string targetScene = "Menu Start"; // Nome da cena a ser carregada

    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer para controlar a opacidade

    void Start()
    {
        // Obtém o SpriteRenderer anexado
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Objeto precisa de um SpriteRenderer!");
            return;
        }
        

        // Inicializa a opacidade da imagem em 0
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        // Garante que o botão esteja inicialmente desativado
        if (sceneButton != null)
        {
            sceneButton.gameObject.SetActive(false);
            sceneButton.onClick.AddListener(OnButtonClicked);
        }

        // Inicia a sequência de exibição da imagem e do botão
        StartCoroutine(AppearSequence());
    }

    private IEnumerator AppearSequence()
    {
        // Aguarda 7 segundos antes de exibir a imagem
        yield return new WaitForSeconds(7f);

        // Gradualmente aumenta a opacidade para 1
        yield return StartCoroutine(FadeTo(1f));

        // Aguarda mais 2 segundos antes de exibir o botão
        yield return new WaitForSeconds(2f);

        // Ativa o botão
        if (sceneButton != null)
        {
            sceneButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator FadeTo(float targetOpacity)
    {
        float startOpacity = spriteRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / fadeDuration);

            // Aplica a nova opacidade
            Color color = spriteRenderer.color;
            color.a = newOpacity;
            spriteRenderer.color = color;

            yield return null;
        }

        // Garante que o valor final seja aplicado
        Color finalColor = spriteRenderer.color;
        finalColor.a = targetOpacity;
        spriteRenderer.color = finalColor;
    }

    private void OnButtonClicked()
    {
        // Carrega a cena especificada
        SceneManager.LoadScene(targetScene);
    }
}
