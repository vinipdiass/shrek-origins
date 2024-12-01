using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    public Animator animator; // Referência ao componente Animator
    public RuntimeAnimatorController newController; // Novo controlador de animação
    public float fadeDuration = 2f; // Tempo para fade in/out

    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer para alterar a opacidade

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Objeto precisa de um SpriteRenderer!");
            return;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Iniciar a sequência
        StartCoroutine(CutsceneSequence());
    }

    private IEnumerator CutsceneSequence()
    {
        // Espera 2 segundos antes de iniciar o fade out
        yield return new WaitForSeconds(2f);

        // Gradualmente reduz a opacidade para 0
        yield return StartCoroutine(FadeTo(0f));

        // Aumenta a escala do personagem
        transform.localScale = new Vector3(1.8f, 1.8f, 1f);

        // Troca o controlador de animação
        if (animator != null && newController != null)
        {
            animator.runtimeAnimatorController = newController;
        }

        // Gradualmente aumenta a opacidade para 1
        yield return StartCoroutine(FadeTo(1f));
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
}
