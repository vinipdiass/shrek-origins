using System.Collections; // Necessário para IEnumerator
using System.Collections.Generic; // Necessário para List<>
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimateUI : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites;
    public float animSpeed = 1f;
    private int index;
    private bool isDone;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(animSpeed);
            index++;
            if (index >= sprites.Count)
                index = 0;

            image.sprite = sprites[index];
        }
    }
}

//Aumentar XP necessario
//Diminuir e sprite no final
//Lobiboss mais pro final
