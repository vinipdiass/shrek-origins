using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Importar o namespace para gerenciar cenas

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI counterText;
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
        timer = 5;
    }

    IEnumerator StartCountdown()
    {
        while (timer > 0)
        {
            if (playerStateMachine.currentHealth <= 0) break;
            counterText.text = timer.ToString(); // Atualiza o texto da UI
            yield return new WaitForSeconds(1); // Espera 1 segundo
            timer--; // Decrementa o timer
        }

        counterText.text = "0"; // Exibe zero quando a contagem terminar

        // Para o jogo
        Time.timeScale = 0;

        // Ativa o painel de fim de jogo ou carrega a cena de final
        if (playerStateMachine.currentHealth <= 0)
        {
            endGamePanel.SetActive(true);
        }
        else
        {
            string playerAbility = PlayerPrefs.GetString("Map", "None");
            if (playerAbility == "Map1")
            {
                GameDataManager.instance.DesbloquearMapa(1);
                victoryPanel.SetActive(true);
            }
            else if (playerAbility == "Map2")
            {
                GameDataManager.instance.DesbloquearMapa(2);
                victoryPanel.SetActive(true);
            }
            else if (playerAbility == "Map3")
            {
                GameDataManager.instance.DesbloquearMapa(3);
                victoryPanel.SetActive(true);
            }
            else if (playerAbility == "Map4")
            {
                Time.timeScale = 1; // Restaura o tempo normal antes de carregar a cena
                SceneManager.LoadScene("FINAL"); // Carrega a cena "FINAL"
            }
        }
    }
}
