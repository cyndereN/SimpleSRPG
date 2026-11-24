using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHideUI : MonoBehaviour
{
    [SerializeField] private Animation animation1;
    [SerializeField] private Animation animation2;
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UASOnBusyChanged;
        TurnSystem.Instance.OnTurnChanged += TSOnTurnChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Hide()
    {
        animation1.Play("HideAnimation1");
        animation2.Play("HideAnimation3");
    }

    private void Show()
    {
        animation1.Play("HideAnimation2");
        animation2.Play("HideAnimation4");
    }

    private void UASOnBusyChanged(object sender,bool isBusy)
    {
        if (isBusy) { Hide(); }
        else { Show(); }
    }

    private void TSOnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn()) { Show(); }
        else { Hide(); }
    }
}
