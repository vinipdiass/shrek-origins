using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject player;
    public TextMeshProUGUI upgradeDescription;
    public List<Button> buttons;
    public PlayerStateMachine playerStateMachine;

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

    public enum Abilities { Punch = 1, Roar = 2, Fart = 3, GasAttack = 4 } //BeetleAttack = 5, //BoomerangAttack = 6}


    void Awake()
    {
        
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

    private void GenerateRandomButtons()
    {
        int numberOfAttacks = GetNumberOfAttacks();
        List<int> availableAbilities = new List<int>();

        if (numberOfAttacks < 3)
        {
            // Player can learn new attacks or evolve existing ones
            for (int i = 1; i <= 6; i++)
            {
                if ((i == 1 && !playerStateMachine.hasPunch) ||
                    (i == 2 && !playerStateMachine.hasRoar) ||
                    (i == 3 && !playerStateMachine.hasFart) ||
                    (i == 4 && !playerStateMachine.hasGasAttack) ||
                    (i == 5 && !playerStateMachine.hasBeetleAttack) ||
                    (i == 6 && !playerStateMachine.hasBoomerangAttack))
                {
                    availableAbilities.Add(i); // Add new attacks to the list
                }
            }
        }

        // Always include already acquired attacks for evolution
        if (playerStateMachine.hasPunch) availableAbilities.Add(1);
        if (playerStateMachine.hasRoar) availableAbilities.Add(2);
        if (playerStateMachine.hasFart) availableAbilities.Add(3);
        if (playerStateMachine.hasGasAttack) availableAbilities.Add(4);
        if (playerStateMachine.hasBeetleAttack) availableAbilities.Add(5);
        if (playerStateMachine.hasBoomerangAttack) availableAbilities.Add(6);

        // Ensure we have at least one ability to offer
        if (availableAbilities.Count == 0)
        {
            Debug.LogError("No abilities available to offer.");
            return;
        }

        // Generate unique random abilities
        HashSet<int> uniqueNumbers = new HashSet<int>();
        System.Random random = new System.Random();
        while (uniqueNumbers.Count < 3 && uniqueNumbers.Count < availableAbilities.Count)
        {
            int index = random.Next(availableAbilities.Count);
            uniqueNumbers.Add(availableAbilities[index]);
        }

        int[] numbers = new int[3];
        uniqueNumbers.CopyTo(numbers);

        // Fill remaining slots with -1 if less than 3 abilities are available
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
    }

    private void SetButtonImage(Button button, int abilityId)
    {
        if (abilityId == -1)
        {
            button.gameObject.SetActive(false); // Se não há habilidade, desativa o botão
            return;
        }

        button.gameObject.SetActive(true); // Ativa o botão e define o sprite
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
                    soco.Evolute();
                    upgradeDescription.text = "Soco evoluído!";
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
                    rugido.Evolute();
                    upgradeDescription.text = "Rugido evoluído!";
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
                    peido.Evolute();
                    upgradeDescription.text = "Peido evoluído!";
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
                    gasAttack.Evolute();
                    upgradeDescription.text = "Ataque de gás evoluído!";
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
                    besouroAttack.Evolute();
                    upgradeDescription.text = "Ataque de besouro evoluído!";
                }
                else
                {
                    playerStateMachine.hasBeetleAttack = true; 
                    besouroAttack.Evolute();
                    Debug.Log("Entrou aqui");
                    upgradeDescription.text = "Ataque de besouro adquirido!";
                }
                break;

            case 6: // Boomerang Attack
                if (playerStateMachine.hasBoomerangAttack)
                {
                    boomerangAttack.Evolute();
                    upgradeDescription.text = "Ataque de bumerangue evoluído!";
                }
                else
                {
                    playerStateMachine.hasBoomerangAttack = true; // Atualiza estado imediatamente
                    upgradeDescription.text = "Ataque de bumerangue adquirido!";
                }
                break;
        }

        // Atualiza os pontos de experiência e progresso
        playerStateMachine.experiencePoints -= (int)playerStateMachine.experiencePointsRequired;
        playerStateMachine.countLevel++;
        playerStateMachine.experiencePointsRequired += 10;

        // Fecha o painel de upgrade
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        buttonsDefined = false;
    }



}

