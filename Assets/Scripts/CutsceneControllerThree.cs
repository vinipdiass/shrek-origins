using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar cenas
using System.Collections;

public class AppearImage2 : MonoBehaviour
{
    public float fadeDuration = 2f; // Duração do fade-in
    public string targetScene = "Thanks"; // Nome da cena a ser carregada
    public SpriteRenderer blackScreen; // Referência ao SpriteRenderer para a tela preta

    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer para controlar a opacidade

    void Start()
    {
        // Obtém o SpriteRenderer anexado para a imagem inicial
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Objeto precisa de um SpriteRenderer!");
            return;
        }

        // Inicializa a opacidade da imagem inicial em 0
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        // Inicializa a tela preta completamente transparente
        if (blackScreen != null)
        {
            Color blackColor = blackScreen.color;
            blackColor.a = 0f;
            blackScreen.color = blackColor;
        }
        else
        {
            Debug.LogError("A tela preta (blackScreen) não foi atribuída!");
        }

        // Inicia a sequência de exibição da imagem e da tela preta
        StartCoroutine(AppearSequence());
    }

    private IEnumerator AppearSequence()
    {
        // Aguarda 7 segundos antes de exibir a imagem
        yield return new WaitForSeconds(7f);

        // Gradualmente aumenta a opacidade da imagem inicial para 1
        yield return StartCoroutine(FadeSprite(spriteRenderer, 1f));

        // Aguarda mais 3 segundos antes de iniciar a tela preta
        yield return new WaitForSeconds(3f);

        // Gradualmente torna a tela preta
        if (blackScreen != null)
        {
            yield return StartCoroutine(FadeSprite(blackScreen, 1f));
        }

        // Aguarda 2 segundos com a tela preta totalmente opaca
        yield return new WaitForSeconds(2f);

        // Carrega a cena especificada
        SceneManager.LoadScene(targetScene);
    }

    private IEnumerator FadeSprite(SpriteRenderer targetRenderer, float targetOpacity)
    {
        float startOpacity = targetRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / fadeDuration);

            // Aplica a nova opacidade ao SpriteRenderer
            Color color = targetRenderer.color;
            color.a = newOpacity;
            targetRenderer.color = color;

            yield return null;
        }

        // Garante que o valor final seja aplicado
        Color finalColor = targetRenderer.color;
        finalColor.a = targetOpacity;
        targetRenderer.color = finalColor;
    }
}
