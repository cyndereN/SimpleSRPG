using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BideAction : BaseAction
{
    
    protected override void Awake()
    {
        base.Awake();

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        animator.SetTrigger("IsBide");
        Invoke("ActionEnd", 3.0f);
    }

    public override string GetActionName()
    {
        return "待机";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition gridPosition = unit.GetGridPosition();
        return new List<GridPosition> { gridPosition };
    }

    public override void ActionEnd()
    {
        onActionComplete();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
