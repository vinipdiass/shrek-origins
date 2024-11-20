using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public PlayerData playerData;

    private string saveFilePath;

    private void Awake()
    {
        // Implementação do Singleton
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        saveFilePath = Application.persistentDataPath + "/playerData.json";
        LoadData();
    }

    // Salva os dados em um arquivo JSON
    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Dados salvos em: " + saveFilePath);
    }

    // Carrega os dados do arquivo JSON
    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Dados carregados com sucesso.");
        }
        else
        {
            playerData = new PlayerData(); // Inicializa com dados padrão
            Debug.Log("Nenhum dado salvo encontrado, inicializando com valores padrão.");
        }
    }

    // Exemplo de função para adicionar mapa desbloqueado
    public void DesbloquearMapa(int index)
    {
        if (index >= 0 && index < playerData.mapasDesbloqueados.Count)
        {
            playerData.mapasDesbloqueados[index] = true;
            SaveData();
        }
        else
        {
            Debug.LogError("Índice de mapa inválido para desbloquear.");
        }
    }

    // Exemplo de função para adicionar dinheiro
    public void AdicionarDinheiro(int quantia)
    {
        playerData.dinheiro += quantia;
        SaveData();
    }

    public void AdicionaAtributo(int index)
    {

    }

    public int getMoney()
    {
        return playerData.dinheiro;
    }

    public List<int> getAtributos()
    {
        return playerData.atributosDisponiveis;
    }


    public List<bool> getMapas()
    {
        return playerData.mapasDesbloqueados;
    }
}
