using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LojaDeAtributos : MonoBehaviour
{
    public TextMeshProUGUI moedasTexto;
    public Button atributo1Button;
    public Button atributo2Button;
    public Button atributo3Button;
    public Button atributo4Button;
    public Button atributo5Button;

    private int custoAtributo = 100;
    private int[] comprasAtributo = new int[5]; // Armazena quantas vezes cada atributo foi comprado

    void Start()
    {
        if (GameDataManager.instance.playerData.atributosDisponiveis.Count > 0)
        {
            // Sincronizar os atributos comprados com os valores salvos
            for (int i = 0; i < comprasAtributo.Length; i++)
            {
                if (i < GameDataManager.instance.playerData.atributosDisponiveis.Count)
                {
                    comprasAtributo[i] = GameDataManager.instance.playerData.atributosDisponiveis[i];
                }
            }
        }

        AtualizarTextoMoedas();
        ConfigurarBotoes();

        // Desativar botões se o atributo já estiver no nível máximo
        for (int i = 0; i < comprasAtributo.Length; i++)
        {
            if (comprasAtributo[i] >= 3)
            {
                DesativarBotao(i);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetarAtributos();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameDataManager.instance.playerData.dinheiro = 0;
            AtualizarTextoMoedas();
        }
    }



    void AtualizarTextoMoedas()
    {
        moedasTexto.text = "" + GameDataManager.instance.getMoney();
    }

    void ConfigurarBotoes()
    {
        atributo1Button.onClick.AddListener(() => ComprarAtributo(0));
        atributo2Button.onClick.AddListener(() => ComprarAtributo(1));
        atributo3Button.onClick.AddListener(() => ComprarAtributo(2));
        atributo4Button.onClick.AddListener(() => ComprarAtributo(3));
        atributo5Button.onClick.AddListener(() => ComprarAtributo(4));
    }

    void ComprarAtributo(int indiceAtributo)
    {
        int custoCalculado = custoAtributo * (comprasAtributo[indiceAtributo] + 1);
        if (GameDataManager.instance.playerData.dinheiro >= custoCalculado && comprasAtributo[indiceAtributo] < 3)
        {
            // Deduzir o custo das moedas do jogador
            GameDataManager.instance.playerData.dinheiro -= custoCalculado;

            // Atualizar os atributos na lista de atributos disponíveis
            if (GameDataManager.instance.playerData.atributosDisponiveis.Count <= indiceAtributo)
            {
                for (int i = GameDataManager.instance.playerData.atributosDisponiveis.Count; i <= indiceAtributo; i++)
                {
                    GameDataManager.instance.playerData.atributosDisponiveis.Add(0); // Preencher valores inexistentes
                }
            }

            GameDataManager.instance.playerData.atributosDisponiveis[indiceAtributo] += 1;
            comprasAtributo[indiceAtributo]++;

            AtualizarTextoMoedas();
            GameDataManager.instance.SaveData();

            // Verificar se o limite de compra foi atingido e desativar o botão se necessário
            if (comprasAtributo[indiceAtributo] >= 3)
            {
                DesativarBotao(indiceAtributo);
            }
        }
        else
        {
            Debug.Log("Moedas insuficientes ou limite de compra atingido para este atributo.");
        }
    }
    void ResetarAtributos()
    {
        // Zerar os atributos na lista de GameDataManager
        for (int i = 0; i < comprasAtributo.Length; i++)
        {
            comprasAtributo[i] = 0;

            if (GameDataManager.instance.playerData.atributosDisponiveis.Count > i)
            {
                GameDataManager.instance.playerData.atributosDisponiveis[i] = 0;
            }
        }

        // Adicionar 1500 de dinheiro
        GameDataManager.instance.playerData.dinheiro += 1500;

        // Atualizar a interface
        AtualizarTextoMoedas();

        // Reativar botões desativados
        for (int i = 0; i < comprasAtributo.Length; i++)
        {
            AtivarBotao(i);
        }

        // Salvar os dados
        GameDataManager.instance.SaveData();

        Debug.Log("Atributos resetados e 1500 moedas adicionadas!");
    }



    void DesativarBotao(int indiceAtributo)
    {
        switch (indiceAtributo)
        {
            case 0: atributo1Button.interactable = false; break;
            case 1: atributo2Button.interactable = false; break;
            case 2: atributo3Button.interactable = false; break;
            case 3: atributo4Button.interactable = false; break;
            case 4: atributo5Button.interactable = false; break;
        }
    }

    void AtivarBotao(int indiceAtributo)
    {
        switch (indiceAtributo)
        {
            case 0: atributo1Button.interactable = true; break;
            case 1: atributo2Button.interactable = true; break;
            case 2: atributo3Button.interactable = true; break;
            case 3: atributo4Button.interactable = true; break;
            case 4: atributo5Button.interactable = true; break;
        }
    }

}
