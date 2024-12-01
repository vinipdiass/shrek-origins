using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MapSelect : MonoBehaviour
{
    public Button map1;
    public Button map2;
    public Button map3;
    public Button map4;
    public TextMeshProUGUI mapText1;
    public TextMeshProUGUI mapText2;
    public TextMeshProUGUI mapText3;
    public TextMeshProUGUI mapText4;

    private List<Button> mapButtons;
    private List<TextMeshProUGUI> mapTexts;
    public Button retornarButton;

    private void Start()
    {
        // Adiciona os botões e textos à lista para facilitar o acesso
        mapButtons = new List<Button> { map1, map2, map3, map4 };
        mapTexts = new List<TextMeshProUGUI> { mapText1, mapText2, mapText3, mapText4 };

        // Garante que os mapas tenham valores iniciais e o primeiro mapa esteja desbloqueado
        EnsureDefaultMapUnlocked();

        // Atualiza o estado dos botões e textos baseado nos mapas desbloqueados
        UpdateMapButtons();
        retornarButton.onClick.AddListener(() => Retornar());
    }

    private void Update()
    {
        // Detecta a tecla R para resetar os mapas
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMapsToInitialState();
        }
        
    }

    private void EnsureDefaultMapUnlocked()
    {
        // Garante que os mapas desbloqueados estão inicializados corretamente
        List<bool> mapasDesbloqueados = GameDataManager.instance.playerData.mapasDesbloqueados;

        if (mapasDesbloqueados.Count == 0)
        {
            // Inicializa todos os mapas como bloqueados
            for (int i = 0; i < mapButtons.Count; i++)
            {
                mapasDesbloqueados.Add(false);
            }

            // Desbloqueia o primeiro mapa
            mapasDesbloqueados[0] = true;

            // Salva os dados atualizados
            GameDataManager.instance.SaveData();
        }
    }

    public void UpdateMapButtons()
    {
        List<bool> mapasDesbloqueados = GameDataManager.instance.playerData.mapasDesbloqueados;

        for (int i = 0; i < mapButtons.Count; i++)
        {
            if (i < mapasDesbloqueados.Count)
            {
                // Define se o botão está interativo com base no estado de desbloqueio
                mapButtons[i].interactable = mapasDesbloqueados[i];

                // Atualiza o texto apenas se o mapa estiver bloqueado
                if (!mapasDesbloqueados[i])
                {
                    mapTexts[i].text = "Mapa Bloqueado";
                }
            }
            else
            {
                // Caso o mapa não exista, mantém o botão desativado e define o texto como bloqueado
                mapButtons[i].interactable = false;
                mapTexts[i].text = "Mapa Bloqueado";
            }
        }
    }

    public void DesbloquearMapa(int index)
    {
        List<bool> mapasDesbloqueados = GameDataManager.instance.playerData.mapasDesbloqueados;

        if (index >= 0 && index < mapasDesbloqueados.Count)
        {
            // Desbloqueia o mapa correspondente
            mapasDesbloqueados[index] = true;

            // Salva os dados atualizados
            GameDataManager.instance.SaveData();

            // Atualiza os botões e textos
            UpdateMapButtons();
        }
        else
        {
            Debug.LogError("Índice de mapa inválido para desbloquear.");
        }
    }

    public void ResetMapsToInitialState()
    {
        // Reseta todos os mapas ao estado inicial
        List<bool> mapasDesbloqueados = GameDataManager.instance.playerData.mapasDesbloqueados;

        for (int i = 0; i < mapButtons.Count; i++)
        {
            mapasDesbloqueados[i] = false; // Bloqueia todos os mapas
        }

        mapasDesbloqueados[0] = true; // Garante que o primeiro mapa está desbloqueado

        // Salva os dados atualizados
        GameDataManager.instance.SaveData();

        // Atualiza os botões e textos
        UpdateMapButtons();

        Debug.Log("Mapas resetados ao estado inicial.");
    }

    public void Map1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select Attack");
        PlayerPrefs.SetString("Map", "Map1");
    }

    public void Retornar()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Start");
    }

    public void Map2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select Attack");
        PlayerPrefs.SetString("Map", "Map2");
    }

    public void Map3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select Attack");
        PlayerPrefs.SetString("Map", "Map3");
    }

    public void Map4()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select Attack");
        PlayerPrefs.SetString("Map", "Map4");
    }
}
