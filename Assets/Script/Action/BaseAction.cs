using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Animator animator;
    protected Unit unit;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        unit = GetComponent<Unit>();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
         
    }
    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract void ActionEnd();

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    public virtual int GetActionPointsCost(){ return 1; }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();

        foreach(GridPosition gridPosition in validGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if(enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((x, y) => x.actionValue - y.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            return null;
        }
    }
}
