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

        upgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStateMachine.experiencePoints >= playerStateMachine.experiencePointsRequired)
        {
            Time.timeScale = 0;
            upgradePanel.SetActive(true);
            

            buttons[0].GetComponent<Image>().sprite = soco.normalSprite;
            buttons[1].GetComponent<Image>().sprite = rugido.normalSprite;
            buttons[2].GetComponent<Image>().sprite = peido.normalSprite;    
        }
    }

    public void Button1()
    {
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
        
        
        playerStateMachine.experiencePoints = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
    }

    public void Button2()
    {
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

        playerStateMachine.experiencePoints = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);

    }

    public void Button3()
    {
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

        playerStateMachine.experiencePoints = 0;
        Time.timeScale = 1;
        upgradePanel.SetActive(false);
    }
}

