using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LojaDeAtributos : MonoBehaviour
{
    public TextMeshProUGUI moedasTexto;
    public Button atributo1Button;
    public Button atributo2Button;
    public Button atributo3Button;

    private int custoAtributo = 100; // Define o custo de cada atributo
    private int[] comprasAtributo = new int[3]; // Armazena quantas vezes cada atributo foi comprado

    void Start()
    {
        AtualizarTextoMoedas();
        ConfigurarBotoes();
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
    }

    void ComprarAtributo(int indiceAtributo)
    {
        if (GameDataManager.instance.playerData.dinheiro >= custoAtributo && comprasAtributo[indiceAtributo] < 3)
        {
            // Deduzir o custo das moedas do jogador
            GameDataManager.instance.playerData.dinheiro -= custoAtributo;
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

    void DesativarBotao(int indiceAtributo)
    {
        switch (indiceAtributo)
        {
            case 0: atributo1Button.interactable = false; break;
            case 1: atributo2Button.interactable = false; break;
            case 2: atributo3Button.interactable = false; break;
        }
    }
}
