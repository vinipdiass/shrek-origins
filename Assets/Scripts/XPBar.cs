using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    public Image XPBarImage;
    public PlayerStateMachine playerStateMachine;

    void Awake()
    {
        playerStateMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        XPBarImage.fillAmount = playerStateMachine.experiencePoints / playerStateMachine.experiencePointsRequired;
    }
}
