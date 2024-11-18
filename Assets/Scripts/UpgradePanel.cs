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

    public enum Abilities {Punch = 1, Roar = 2, Fart = 3, GasAttack = 4} //BeetleAttack = 5, //BoomerangAttack = 6}


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

            // Gerar números aleatórios distintos entre 1 e 6
            HashSet<int> uniqueNumbers = new HashSet<int>();
            System.Random random = new System.Random();
            while (uniqueNumbers.Count < 3) uniqueNumbers.Add(random.Next(1, 7)); // Gera número entre 1 e 6

            // Atribuir os números às variáveis
            int[] numbers = new int[3];
            uniqueNumbers.CopyTo(numbers);
            button1Random = numbers[0];
            button2Random = numbers[1];
            button3Random = numbers[2];

            // Debug para verificar os valores
            //Debug.Log($"Button1Random: {button1Random}, Button2Random: {button2Random}, Button3Random: {button3Random}");

            if(!buttonsDefined) DefineButtons();
        }
    }

    public void DefineButtons()
    {
        buttonsDefined = true;
        switch (button1Random)
        {
            case 1:
                buttons[0].GetComponent<Image>().sprite = soco.normalSprite;
                break;
            case 2:
                buttons[0].GetComponent<Image>().sprite = rugido.normalSprite;
                break;
            case 3:
                buttons[0].GetComponent<Image>().sprite = peido.normalSprite;
                break;
            case 4:
                buttons[0].GetComponent<Image>().sprite = gasAttack.normalSprite;
                break;
            /*case 5:
                buttons[0].GetComponent<Image>().sprite = besouroAttack.normalSprite;
                break;
            case 6:
                buttons[0].GetComponent<Image>().sprite = boomerangAttack.normalSprite;
                break;*/
        }

        switch (button2Random)
        {
            case 1:
                buttons[1].GetComponent<Image>().sprite = soco.normalSprite;
                break;
            case 2:
                buttons[1].GetComponent<Image>().sprite = rugido.normalSprite;
                break;
            case 3:
                buttons[1].GetComponent<Image>().sprite = peido.normalSprite;
                break;
            case 4:
                buttons[1].GetComponent<Image>().sprite = gasAttack.normalSprite;
                break;
            /*case 5:
                buttons[1].GetComponent<Image>().sprite = besouroAttack.normalSprite;
                break;
            case 6:
                buttons[1].GetComponent<Image>().sprite = boomerangAttack.normalSprite;
                break;*/
        }

        switch (button3Random)
        {
            case 1:
                buttons[2].GetComponent<Image>().sprite = soco.normalSprite;
                break;
            case 2:
                buttons[2].GetComponent<Image>().sprite = rugido.normalSprite;
                break;
            case 3:
                buttons[2].GetComponent<Image>().sprite = peido.normalSprite;
                break;
            case 4:
                buttons[2].GetComponent<Image>().sprite = gasAttack.normalSprite;
                break;
            /*case 5:
                buttons[2].GetComponent<Image>().sprite = besouroAttack.normalSprite;
                break;
            case 6:
                buttons[2].GetComponent<Image>().sprite = boomerangAttack.normalSprite;
                break;*/
        }
    }


    public void Button1()
    {
        switch (button1Random)
        {
            case 1:
                if (playerStateMachine.hasPunch)
                {
                    soco.evolute();
                    upgradeDescription.text = "Soco evoluído!";
                }
                else
                {
                    playerStateMachine.hasPunch = true;
                    upgradeDescription.text = "Soco adquirido!";
                }
                break;
            case 2:
                if (playerStateMachine.hasRoar)
                {
                    rugido.evolute();
                    upgradeDescription.text = "Rugido evoluído!";
                }
                else
                {
                    playerStateMachine.hasRoar = true;
                    upgradeDescription.text = "Rugido adquirido!";
                }
                break;
            case 3:
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
            case 4:
                if (playerStateMachine.hasGasAttack)
                {
                    gasAttack.evolute();
                    upgradeDescription.text = "Ataque de gás evoluido!";
                }
                else
                {
                    playerStateMachine.hasGasAttack = true;
                    upgradeDescription.text = "Ataque de gás adquirido!";
                }
                break;
            case 5:
                if (playerStateMachine.hasBeetleAttack)
                {
                    besouroAttack.evolute();
                    upgradeDescription.text = "Ataque de besouro evoluído!";
                }
                else
                {
                    playerStateMachine.hasBeetleAttack = true;
                    upgradeDescription.text = "Ataque de besouro adquirido!";
                }
                break;
            case 6:
                if (playerStateMachine.hasBoomerangAttack)
                {
                    boomerangAttack.evolute();
                    upgradeDescription.text = "Ataque de bumerangue evoluído!";
                }
                else
                {
                    playerStateMachine.hasBoomerangAttack = true;
                    upgradeDescription.text = "Ataque de bumerangue adquirido!";
                }
                break;
        }

        playerStateMachine.experiencePoints = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        buttonsDefined = false;
    }

    public void Button2()
    {
        switch (button2Random)
        {
            case 1:
                if (playerStateMachine.hasPunch)
                {
                    soco.evolute();
                    upgradeDescription.text = "Soco evoluído!";
                }
                else
                {
                    playerStateMachine.hasPunch = true;
                    upgradeDescription.text = "Soco adquirido!";
                }
                break;
            case 2:
                if (playerStateMachine.hasRoar)
                {
                    rugido.evolute();
                    upgradeDescription.text = "Rugido evoluído!";
                }
                else
                {
                    playerStateMachine.hasRoar = true;
                    upgradeDescription.text = "Rugido adquirido!";
                }
                break;
            case 3:
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
            case 4:
                if (playerStateMachine.hasGasAttack)
                {
                    gasAttack.evolute();
                    upgradeDescription.text = "Ataque de gás evoluido!";
                }
                else
                {
                    playerStateMachine.hasGasAttack = true;
                    upgradeDescription.text = "Ataque de gás adquirido!";
                }
                break;
            case 5:
                if (playerStateMachine.hasBeetleAttack)
                {
                    besouroAttack.evolute();
                    upgradeDescription.text = "Ataque de besouro evoluído!";
                }
                else
                {
                    playerStateMachine.hasBeetleAttack = true;
                    upgradeDescription.text = "Ataque de besouro adquirido!";
                }
                break;
            case 6:
                if (playerStateMachine.hasBoomerangAttack)
                {
                    boomerangAttack.evolute();
                    upgradeDescription.text = "Ataque de bumerangue evoluído!";
                }
                else
                {
                    playerStateMachine.hasBoomerangAttack = true;
                    upgradeDescription.text = "Ataque de bumerangue adquirido!";
                }
                break;
        }

        playerStateMachine.experiencePoints = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        buttonsDefined = false;
    }

    public void Button3()
    {
        switch (button3Random)
        {
            case 1:
                if (playerStateMachine.hasPunch)
                {
                    soco.evolute();
                    upgradeDescription.text = "Soco evoluído!";
                }
                else
                {
                    playerStateMachine.hasPunch = true;
                    upgradeDescription.text = "Soco adquirido!";
                }
                break;
            case 2:
                if (playerStateMachine.hasRoar)
                {
                    rugido.evolute();
                    upgradeDescription.text = "Rugido evoluído!";
                }
                else
                {
                    playerStateMachine.hasRoar = true;
                    upgradeDescription.text = "Rugido adquirido!";
                }
                break;
            case 3:
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
            case 4:
                if (playerStateMachine.hasGasAttack)
                {
                    gasAttack.evolute();
                    upgradeDescription.text = "Ataque de gás evoluido!";
                }
                else
                {
                    playerStateMachine.hasGasAttack = true;
                    upgradeDescription.text = "Ataque de gás adquirido!";
                }
                break;
            case 5:
                if (playerStateMachine.hasBeetleAttack)
                {
                    besouroAttack.evolute();
                    upgradeDescription.text = "Ataque de besouro evoluído!";
                }
                else
                {
                    playerStateMachine.hasBeetleAttack = true;
                    upgradeDescription.text = "Ataque de besouro adquirido!";
                }
                break;
            case 6:
                if (playerStateMachine.hasBoomerangAttack)
                {
                    boomerangAttack.evolute();
                    upgradeDescription.text = "Ataque de bumerangue evoluído!";
                }
                else
                {
                    playerStateMachine.hasBoomerangAttack = true;
                    upgradeDescription.text = "Ataque de bumerangue adquirido!";
                }
                break;
        }

        playerStateMachine.experiencePoints = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
        buttonsDefined = false;
    }
}

