using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar cenas
using System.Collections;

public class AppearImage : MonoBehaviour
{
    public float fadeDuration = 2f; // Duração do fade-in
    public string targetScene = "Thanks"; // Nome da cena a ser carregada

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

        // Inicia a sequência de exibição da imagem
        StartCoroutine(AppearSequence());
    }

    private IEnumerator AppearSequence()
    {
        // Aguarda 7 segundos antes de exibir a imagem
        yield return new WaitForSeconds(7f);

        // Gradualmente aumenta a opacidade da imagem inicial para 1
        yield return StartCoroutine(FadeSprite(spriteRenderer, 1f));

        // Aguarda mais 3 segundos antes de carregar a cena
        yield return new WaitForSeconds(3f);

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
