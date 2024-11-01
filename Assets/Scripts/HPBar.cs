using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    public Image HPBarImage;
    public TextMeshProUGUI HPValue;
    public PlayerStateMachine playerStateMachine;

    void Awake()
    {
        playerStateMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        HPValue.text = playerStateMachine.currentHealth + "/" + playerStateMachine.maxHealth;
        HPBarImage.fillAmount = playerStateMachine.currentHealth / playerStateMachine.maxHealth;
    }
}
