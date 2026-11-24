using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    public  int MAX_ACTION_POINTS = 4;
    private GridPosition gridPosition;

    private MoveAction moveAction;
    private BideAction bideAction;
    private AttackAction attackAction;
    private BaseAction[] baseActionArray;

    private int actionPoints = 4;

    private HealthSystem healthSystem;

    [SerializeField]private bool isEnemy = false;

    void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        bideAction = GetComponent<BideAction>();
        attackAction = GetComponent<AttackAction>();
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Start()
    {
        actionPoints = MAX_ACTION_POINTS;

        TurnSystem.Instance.OnTurnChanged += TSOnTurnChanged;
        healthSystem.UnitDead += HSUnitDead;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }



    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public BideAction GetBideAction()
    {
        return bideAction;
    }

    public AttackAction GetAttackAction()
    {
        return attackAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool CanSpendActionPoints(BaseAction action)
    {
        return actionPoints >= action.GetActionPointsCost();
    }

    private void SpendPoints(int points)
    {
        actionPoints -= points;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty); 
    }

    public bool TrySpendActionPoints(BaseAction action)
    {
        if (CanSpendActionPoints(action))
        {
            SpendPoints(action.GetActionPointsCost());
            return true;
        }
        else { return false; }
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TSOnTurnChanged(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {     
            actionPoints = MAX_ACTION_POINTS;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HSUnitDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

        Destroy(gameObject);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public Vector3 GetWorldPosition()
    { 
        return transform.position;
    }


}
 