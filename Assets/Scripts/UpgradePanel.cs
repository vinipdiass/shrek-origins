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

    void Awake()
    {
        upgradePanel = GameObject.Find("UpgradePanel");
        upgradeDescription = GameObject.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UpgradeButton")) buttons.Add(obj.GetComponent<Button>());
        player = GameObject.Find("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        upgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStateMachine.experiencePoints >= playerStateMachine.experiencePointsRequired)
        {
            Time.timeScale = 0;
            upgradePanel.SetActive(true);
            Debug.Log("Entrou aqui");
        }
    }
}
