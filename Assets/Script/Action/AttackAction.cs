using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BaseAction
{
    private enum State
    {
        Prepare,
        Attack,
        Finish,
    }

    private bool isActive = false;
    private bool canAttack = false;
    private int maxAttackDistance = 1;
    private float stateTimer = 0;
    private Unit targetUnit;

    private State state = State.Prepare;

    protected override void Update()
    {
        if (!isActive) { return; }
        
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Prepare:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, 0.3f);
                break;
            case State.Attack:
                if(canAttack)
                {
                    DoAttack();
                    canAttack = false;
                }
                break;
            case State.Finish:

                break;
        }

        if(stateTimer <= 0)
        {
            NextStste();
        }
    }

    public void DoAttack()
    {
        animator.SetTrigger("Attack");
        Invoke("AttackInvoke", 0.7f);
    }

    public void NextStste()
    {
        float attackTime = 1f;
        float finishTime = 1f;
        switch (state)
        {
            case State.Prepare:
                state = State.Attack;
                stateTimer = attackTime;
                break;
            case State.Attack:
                state = State.Finish;
                stateTimer = finishTime;
                break;
            case State.Finish:
                ActionEnd();
                break;
        }
        
    }

    public override string GetActionName()
    {
        return "攻击";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }


    public  List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
            {
                GridPosition offectGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offectGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxAttackDistance)
                {
                    continue;
                }


                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        canAttack = true;

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Prepare;
        float prepareTime = 0.5f;
        stateTimer = prepareTime;

    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override void ActionEnd()
    {
        isActive = false;
        onActionComplete();
    }

    public void AttackInvoke()
    {
        targetUnit.TakeDamage(50);
        SoundManger.Instance.PlayShotSound(0);
    }

    public int GetAttackRange()
    {
        return maxAttackDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
