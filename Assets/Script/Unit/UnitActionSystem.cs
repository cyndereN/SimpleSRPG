using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnSelectedActionStarted;

    private Unit selectedUnit = null;

    private bool isBusy = false;

    private BaseAction selectedAction;

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem in the scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedUnit = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBusy) return;

        if(!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.IsPointerOverGameObject()) {  return; }

        if (TryHandUnitSelection()) return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (!selectedUnit) return;
            
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
                
        if(!selectedUnit.TrySpendActionPoints(selectedAction)) return;

        SetBusyTrue();
        selectedAction.TakeAction(mouseGridPosition, SetBusyFalse);

        OnSelectedActionStarted?.Invoke(this, EventArgs.Empty);

    }

    private bool TryHandUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Unit"))) return false;
        
        Unit unit = hit.transform.GetComponent<Unit>();

        if (!unit) return false;

        if (unit == selectedUnit) return false;

        if(unit.IsEnemy()) return false;

        SetSelectedUnit(unit);
        OnBusyChanged?.Invoke(this, false);
        return true;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetBusyTrue()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    public void SetBusyFalse()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }




}

