using UnityEngine;

public class HPBar2 : MonoBehaviour
{
    public SpriteRenderer HPBarImage; // O sprite que será escalado
    public PlayerStateMachine playerStateMachine;

    void Awake()
    {
        playerStateMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        // Calcula a proporção da vida atual em relação à vida máxima
        float healthPercentage = playerStateMachine.currentHealth / playerStateMachine.maxHealth;

        // Ajusta a escala do sprite para refletir a proporção da vida
        HPBarImage.transform.localScale = new Vector3((healthPercentage / 2), 0.5f, 0.5f);
    }
}
