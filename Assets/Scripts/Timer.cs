using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    //public GameObject victoryPanel;
    public GameObject endGamePanel;
    public GameObject victoryPanel;
    public PlayerStateMachine playerStateMachine;
    private int timer = 300;

    public void Awake()
    {
        playerStateMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    void Start()
    {     
        Time.timeScale = 1;
        StartCoroutine(StartCountdown());
        endGamePanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    IEnumerator StartCountdown()
    {
         

        while (timer > 0)
        {
            if(playerStateMachine.currentHealth <= 0) break;
            counterText.text = timer.ToString(); // Atualiza o texto da UI
            yield return new WaitForSeconds(1); // Espera 1 segundo
            timer--; // Decrementa o timer
        }

        counterText.text = "0"; // Exibe zero quando a contagem terminar

        // Para o jogo
        Time.timeScale = 0;
        // Ativa o painel de fim de jogo
        victoryPanel.SetActive(true);

    }
}
