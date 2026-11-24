using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField]private Button endTurnButton;
    [SerializeField]private Text turnText;
    [SerializeField]private Text turnEnemyText;

    void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TSOnTurnChanged;

        UpdataTurnText();
        UpdataTurnButton();

    }

    void Update()
    {
        
    }

    private void TSOnTurnChanged(object sender,EventArgs e)
    {
        UpdataTurnText();
        UpdataTurnButton();
    }

    private void UpdataTurnText()
    {
        turnText.text = "回合 " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdataTurnButton()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) 
        { 
            endTurnButton.gameObject.SetActive(true);
            turnEnemyText.gameObject.SetActive(false);
        }
        else 
        {
            endTurnButton.gameObject.SetActive(false);
            turnEnemyText.gameObject.SetActive(true);
        }
    }

}
