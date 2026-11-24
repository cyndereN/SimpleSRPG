using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField]private GameObject buttonPerfab;
    [SerializeField]private GameObject buttonTransfrom;
    public GameObject PointsBackground1;
    public Text PointsText1;
     
    private List<ActionButtonUI> actionButtonUIList;

    // Start is called before the first frame update
    void Start()
    {
        //PointsBackground.SetActive(false);
        actionButtonUIList = new List<ActionButtonUI>();
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitASOnChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += ActionASOnChanged;
        UnitActionSystem.Instance.OnSelectedActionStarted += ActionStarted;
        TurnSystem.Instance.OnTurnChanged += TSOnTurnChanged;
        Unit.OnAnyActionPointsChanged += UAnyActionPointsChanged;
    }
     
    // Update is called once per frame
    void Update()
    {
         
    }

    private void CreatUnitActionButtons()
    {
        foreach(Transform button in buttonTransfrom.transform)
        {
            Destroy(button.gameObject);
            actionButtonUIList.Clear();
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (!selectedUnit) return;
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
           GameObject button = Instantiate(buttonPerfab, buttonTransfrom.transform);
            ActionButtonUI actionButtonUI = button.GetComponent<ActionButtonUI>();
            if (actionButtonUI) { actionButtonUI.SetBaseAction(baseAction);
                actionButtonUIList.Add(actionButtonUI);
            }
        }
    }

    private void UnitASOnChanged(object sender,EventArgs e)
    {
        CreatUnitActionButtons();
        UpdateSelectedVisual();
        UpdataActionPoints();
    }
    private void ActionASOnChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void ActionStarted(object sender, EventArgs e)
    {
        UpdataActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdataActionPoints()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit(); 
        if (!unit) return;

        if(PointsBackground1)
        PointsBackground1.SetActive(true);

        if (PointsText1)
        PointsText1.text = "行动点数" + unit.GetActionPoints().ToString();
    }

    private void TSOnTurnChanged(object sender, EventArgs e)
    {
        UpdataActionPoints();
    }

    private void UAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdataActionPoints();
    }

    private void OnDestroy()
    {
        Unit.OnAnyActionPointsChanged -= UAnyActionPointsChanged;
        TurnSystem.Instance.OnTurnChanged -= TSOnTurnChanged;
    }

}
