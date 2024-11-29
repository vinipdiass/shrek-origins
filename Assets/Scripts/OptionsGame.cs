using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button returnToMenuButton;

    private bool isPaused = false;

    void Awake()
    {
        // Localiza o painel e os botões no jogo

        // Configura os eventos dos botões
        resumeButton.onClick.AddListener(ResumeGame);
        returnToMenuButton.onClick.AddListener(ReturnToMainMenu);

        // Esconde o menu no início do jogo
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Verifica se o jogador pressionou "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Teste");
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0; // Pausa o jogo
        isPaused = true;
        pauseMenuPanel.SetActive(true); // Mostra o menu
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Retoma o jogo
        isPaused = false;
        pauseMenuPanel.SetActive(false); // Esconde o menu
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Garante que o tempo volte ao normal
        // Carrega a cena do menu principal (substitua "MainMenu" pelo nome da sua cena de menu)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Start");
    }
}
