using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject player;
    public TextMeshProUGUI upgradeDescription;
    public List<Button> buttons;
    public PlayerStateMachine playerStateMachine;

    // Adicione no início da classe
    private Dictionary<int, int> abilityLevels;
    private Dictionary<int, string> abilityDescriptions;
    public int maxEvolutionLevel = 3; // Nível máximo de evolução
    public Sprite emptySprite; // Sprite para habilidades "vazias"


    public Punch soco;
    public Roar rugido;
    public Fart peido;
    public Onion gasAttack;
    public BeetleAttack besouroAttack;
    public BoomerangAttack boomerangAttack;

    public int button1Random;
    public int button2Random;
    public int button3Random;
    public bool buttonsDefined;

    public Sprite punchSprite;
    public Sprite roarSprite;
    public Sprite fartSprite;
    public Sprite gasAttackSprite;
    public Sprite beetleAttackSprite;
    public Sprite boomerangAttackSprite;

    //Atributos:

    public Sprite maxHpSprite;
    public Sprite recoverySprite;
    public Sprite cooldownSprite;
    public Sprite moveSpeedSprite;
    public Sprite damageSprite;

    public enum Abilities
    {
        Punch = 1,
        Roar = 2,
        Fart = 3,
        GasAttack = 4,
        BeetleAttack = 5,
        BoomerangAttack = 6,
        MaxHP = 7,
        Recovery = 8,
        Cooldown = 9
    }



    void Awake()
    {
        abilityDescriptions = new Dictionary<int, string>
        {
            { 1, "Evolui ou Aprende o soco." }, // Punch
            { 2, "Evolui ou Aprende o urro." }, // Roar
            { 3, "Evolui ou Aprende o peido." }, // Fart
            { 4, "Evolui ou Aprende a cebola." }, // Gas Attack
            { 5, "Evolui ou Aprende o besouro." }, // Beetle Attack
            { 6, "Evolui ou Aprende os sapos." }, // Boomerang Attack
            { 7, "Aumenta a vida maxima do jogador." }, // MaxHP
            { 8, "Aumenta a regeneracao de vida." }, // Recovery
            { 9, "Reduz o tempo de recarga dos ataques." }, // Cooldown
            { 10, "Aumenta a velocidade de movimento do jogador." }, // MoveSpeed
            { 11, "Aumenta o dano causado por ataques." } // Damage
        };

        abilityLevels = new Dictionary<int, int>
        {
            { 1, 0 }, // Punch
            { 2, 0 }, // Roar
            { 3, 0 }, // Fart
            { 4, 0 }, // GasAttack
            { 5, 0 }, // BeetleAttack
            { 6, 0 }, // BoomerangAttack
            { 7, 0 }, // MaxHP
            { 8, 0 }, // Recovery
            { 9, 0 }, // Cooldown
            { 10, 0 }, // MoveSpeed
            { 11, 0 }  // Damage
        };



        // Elemento do menu
        upgradePanel = GameObject.Find("UpgradePanel");
        upgradeDescription = GameObject.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UpgradeButton")) buttons.Add(obj.GetComponent<Button>());
        // Player
        player = GameObject.Find("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        // Habilidades do player
        soco = player.GetComponent<Punch>();
        rugido = player.GetComponent<Roar>();
        peido = player.GetComponent<Fart>();
        gasAttack = player.GetComponent<Onion>();
        besouroAttack = player.GetComponent<BeetleAttack>();
        boomerangAttack = player.GetComponent<BoomerangAttack>();

        upgradePanel.SetActive(false);
        buttonsDefined = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStateMachine.experiencePoints >= playerStateMachine.experiencePointsRequired)
        {
            Time.timeScale = 0;
            upgradePanel.SetActive(true);

            if (!buttonsDefined)
            {
                // Generate random numbers only once
                GenerateRandomButtons();
                DefineButtons();
                buttonsDefined = true;
            }
        }
    }

    private bool IsAttackAcquired(int abilityId)
    {
        switch (abilityId)
        {
            case 1:
                return playerStateMachine.hasPunch;
            case 2:
                return playerStateMachine.hasRoar;
            case 3:
                return playerStateMachine.hasFart;
            case 4:
                return playerStateMachine.hasGasAttack;
            case 5:
                return playerStateMachine.hasBeetleAttack;
            case 6:
                return playerStateMachine.hasBoomerangAttack;
            default:
                return false;
        }
    }


    private void GenerateRandomButtons()
    {
        List<int> availableUpgrades = new List<int>();

        // Ataques (habilidades) disponíveis
        for (int i = 1; i <= 6; i++)
        {
            if (abilityLevels[i] < maxEvolutionLevel)
            {
                if ((GetNumberOfAttacks() < 3) || IsAttackAcquired(i))
                {
                    availableUpgrades.Add(i);
                }
            }
        }

        // Atributos disponíveis
        for (int i = 7; i <= 11; i++)
        {
            if (abilityLevels[i] < maxEvolutionLevel)
            {
                availableUpgrades.Add(i);
            }
        }

        // Gera upgrades únicos e aleatórios
        HashSet<int> uniqueNumbers = new HashSet<int>();
        System.Random random = new System.Random();
        while (uniqueNumbers.Count < 3 && uniqueNumbers.Count < availableUpgrades.Count)
        {
            int index = random.Next(availableUpgrades.Count);
            uniqueNumbers.Add(availableUpgrades[index]);
        }

        int[] numbers = new int[3];
        uniqueNumbers.CopyTo(numbers);

        // Preenche com -1 se menos de 3 upgrades estão disponíveis
        for (int i = uniqueNumbers.Count; i < 3; i++)
        {
            numbers[i] = -1;
        }

        button1Random = numbers[0];
        button2Random = numbers[1];
        button3Random = numbers[2];
    }



    private int GetNumberOfAttacks()
    {
        int count = 0;
        if (playerStateMachine.hasPunch) count++;
        if (playerStateMachine.hasRoar) count++;
        if (playerStateMachine.hasFart) count++;
        if (playerStateMachine.hasGasAttack) count++;
        if (playerStateMachine.hasBeetleAttack) count++;
        if (playerStateMachine.hasBoomerangAttack) count++;
        return count;
    }






    public void DefineButtons()
    {
        SetButtonImage(buttons[0], button1Random);
        SetButtonImage(buttons[1], button2Random);
        SetButtonImage(buttons[2], button3Random);

        AddButtonHoverEvent(buttons[0], button1Random);
        AddButtonHoverEvent(buttons[1], button2Random);
        AddButtonHoverEvent(buttons[2], button3Random);
    }

    private void AddButtonHoverEvent(Button button, int abilityId)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        trigger.triggers.Clear();

        // Evento PointerEnter (quando o mouse entra no botão)
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) =>
        {
            ShowDescription(abilityId);
        });
        trigger.triggers.Add(pointerEnter);

        // Evento PointerExit (quando o mouse sai do botão)
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) =>
        {
            HideDescription();
        });
        trigger.triggers.Add(pointerExit);
    }

    private void ShowDescription(int abilityId)
    {
        if (abilityDescriptions.ContainsKey(abilityId) && abilityId > 0)
        {
            upgradeDescription.text = abilityDescriptions[abilityId];
        }
        else
        {
            upgradeDescription.text = "";
        }
    }

    private void HideDescription()
    {
        upgradeDescription.text = "";
    }


    private void SetButtonImage(Button button, int abilityId)
    {
        if (abilityId == -1 || (abilityId > 0 && abilityLevels[abilityId] >= maxEvolutionLevel))
        {
            button.gameObject.SetActive(true); // Mostra o botão vazio
            button.GetComponent<Image>().sprite = emptySprite; // Define sprite vazio
            return;
        }

        button.gameObject.SetActive(true); // Ativa o botão
        switch (abilityId)
        {
            case 1:
                button.GetComponent<Image>().sprite = punchSprite;
                break;
            case 2:
                button.GetComponent<Image>().sprite = roarSprite;
                break;
            case 3:
                button.GetComponent<Image>().sprite = fartSprite;
                break;
            case 4:
                button.GetComponent<Image>().sprite = gasAttackSprite;
                break;
            case 5:
                button.GetComponent<Image>().sprite = beetleAttackSprite;
                break;
            case 6:
                button.GetComponent<Image>().sprite = boomerangAttackSprite;
                break;
            case 7:
                button.GetComponent<Image>().sprite = maxHpSprite;
                break;
            case 8:
                button.GetComponent<Image>().sprite = recoverySprite;
                break;
            case 9:
                button.GetComponent<Image>().sprite = cooldownSprite;
                break;
            case 10:
                button.GetComponent<Image>().sprite = moveSpeedSprite;
                break;
            case 11:
                button.GetComponent<Image>().sprite = damageSprite;
                break;
        }
    }





    public void Button1()
    {
        ApplyUpgrade(button1Random);
    }

    public void Button2()
    {
        ApplyUpgrade(button2Random);
    }

    public void Button3()
    {
        ApplyUpgrade(button3Random);
    }

    private void ApplyUpgrade(int abilityId)
    {
        switch (abilityId)
        {
            case 1: // Punch
                if (playerStateMachine.hasPunch)
                {
                    if (abilityLevels[1] < maxEvolutionLevel)
                    {
                        abilityLevels[1]++;
                        soco.Evolute();
                        upgradeDescription.text = "Soco evoluído!";
                    }
                    else
                    {
                        upgradeDescription.text = "Soco já está no nível máximo!";
                    }
                }
                else
                {
                    playerStateMachine.hasPunch = true;
                    upgradeDescription.text = "Soco adquirido!";
                }
                break;

            case 2: // Roar
                if (playerStateMachine.hasRoar)
                {
                    if (abilityLevels[2] < maxEvolutionLevel)
                    {
                        abilityLevels[2]++;
                        rugido.Evolute();
                        upgradeDescription.text = "Rugido evoluído!";
                    }
                    else
                    {
                        upgradeDescription.text = "Rugido já está no nível máximo!";
                    }
                }
                else
                {
                    playerStateMachine.hasRoar = true;
                    upgradeDescription.text = "Rugido adquirido!";
                }
                break;

            case 3: // Fart
                if (playerStateMachine.hasFart)
                {
                    if (abilityLevels[3] < maxEvolutionLevel)
                    {
                        abilityLevels[3]++;
                        peido.Evolute();
                        upgradeDescription.text = "Peido evoluído!";
                    }
                    else
                    {
                        upgradeDescription.text = "Peido já está no nível máximo!";
                    }
                }
                else
                {
                    playerStateMachine.hasFart = true;
                    upgradeDescription.text = "Peido adquirido!";
                }
                break;

            case 4: // Gas Attack
                if (playerStateMachine.hasGasAttack)
                {
                    if (abilityLevels[4] < maxEvolutionLevel)
                    {
                        abilityLevels[4]++;
                        gasAttack.Evolute();
                        upgradeDescription.text = "Ataque de gás evoluído!";
                    }
                    else
                    {
                        upgradeDescription.text = "Ataque de gás já está no nível máximo!";
                    }
                }
                else
                {
                    playerStateMachine.hasGasAttack = true;
                    upgradeDescription.text = "Ataque de gás adquirido!";
                }
                break;

            case 5: // Beetle Attack
                if (playerStateMachine.hasBeetleAttack)
                {
                    if (abilityLevels[5] < maxEvolutionLevel)
                    {
                        abilityLevels[5]++;
                        besouroAttack.Evolute();
                        upgradeDescription.text = "Ataque de besouro evoluído!";
                    }
                    else
                    {
                        upgradeDescription.text = "Ataque de besouro já está no nível máximo!";
                    }
                }
                else
                {
                    playerStateMachine.hasBeetleAttack = true;
                    besouroAttack.Evolute();
                    upgradeDescription.text = "Ataque de besouro adquirido!";
                }
                break;

            case 6: // Boomerang Attack
                if (playerStateMachine.hasBoomerangAttack)
                {
                    if (abilityLevels[6] < maxEvolutionLevel)
                    {
                        abilityLevels[6]++;
                        boomerangAttack.Evolute();
                        upgradeDescription.text = "Ataque de bumerangue evoluído!";
                    }
                    else
                    {
                        upgradeDescription.text = "Ataque de bumerangue já está no nível máximo!";
                    }
                }
                else
                {
                    playerStateMachine.hasBoomerangAttack = true;
                    upgradeDescription.text = "Ataque de bumerangue adquirido!";
                }
                break;
            case 7: // Max HP
                if (abilityLevels[7] < maxEvolutionLevel)
                {
                    abilityLevels[7]++;
                    playerStateMachine.IncreaseAtribute(0);
                    upgradeDescription.text = "Vida máxima aumentada!";
                }
                else
                {
                    upgradeDescription.text = "Vida máxima já está no nível máximo!";
                }
                break;

            case 8:
                if (abilityLevels[8] < maxEvolutionLevel)
                {
                    abilityLevels[8]++;
                    playerStateMachine.IncreaseAtribute(1);
                }
                else
                {
                }
                break;

            case 9: // Cooldown
                if (abilityLevels[9] < maxEvolutionLevel)
                {
                    abilityLevels[9]++;
                    playerStateMachine.IncreaseAtribute(2);
                }
                else
                {
                }
                break;


            case 10: // Movespeed
                if (abilityLevels[10] < maxEvolutionLevel)
                {
                    abilityLevels[10]++;
                    playerStateMachine.IncreaseAtribute(3);
                }
                else
                {
                }
                break;

            case 11: // Dano
                if (abilityLevels[11] < maxEvolutionLevel)
                {
                    abilityLevels[11]++;
                    playerStateMachine.IncreaseAtribute(4);
                }
                else
                {
                }
                break;


            default:
                upgradeDescription.text = "Nenhuma habilidade selecionada.";
                break;

        }

        // Atualiza os pontos de experiência e progresso
        playerStateMachine.experiencePoints = playerStateMachine.experiencePoints % (int)playerStateMachine.experiencePointsRequired;
        playerStateMachine.countLevel++;
        playerStateMachine.experiencePointsRequired *= 1.2f;

        // Fecha o painel de upgrade
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        buttonsDefined = false;
    }





}

