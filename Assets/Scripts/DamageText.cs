using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public float lifetime = 1f; // Duração antes de desaparecer
    public float floatSpeed = 1f; // Velocidade de movimento para cima
    public Vector3 offset = new Vector3(0, 1, 0); // Offset inicial em relação ao inimigo
    public Vector3 randomizeIntensity = new Vector3(0.5f, 0.5f, 0); // Aleatoriedade na posição inicial

    private TextMeshPro textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();

        // Aplica um offset e randomiza a posição inicial
        transform.position += offset;
        transform.position += new Vector3(
            Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z)
        );

        // Inicia a corrotina para desaparecer
        StartCoroutine(FadeOutAndDestroy());
    }

    void Update()
    {
        // Move o texto para cima
        transform.Translate(Vector3.down * floatSpeed * Time.deltaTime);
    }

    public void SetText(string text)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshPro>();
        textMesh.text = text;
    }

    IEnumerator FadeOutAndDestroy()
    {
        float startAlpha = textMesh.color.a;
        float rate = 1.0f / lifetime;
        float progress = 0.0f;

        while (progress < 1.0)
        {
            Color tmpColor = textMesh.color;
            tmpColor.a = Mathf.Lerp(startAlpha, 0, progress);
            textMesh.color = tmpColor;

            progress += rate * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
