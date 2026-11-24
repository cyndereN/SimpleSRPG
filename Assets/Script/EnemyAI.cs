using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private float timer = 0f;

    private State state = State.WaitForEnemyTurn;
    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TSOnTurnChanged;
    }



    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if(timer<=0)
                {    
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn(); 
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TSOnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        state = State.TakingTurn;
        timer = 2f;
    }

    private bool TryTakeEnemyAIAction(Action completeAction)
    {
        foreach(Unit enemyUnit in UnitManger.Instance.GetEnemyUnitList())
        {
            if(TryEnemyAIAction(enemyUnit, completeAction))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryEnemyAIAction(Unit enemyUnit, Action completeAction)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if(!enemyUnit.CanSpendActionPoints(baseAction))
            {
                continue;
            }

            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction(); 
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
            }
        }

        if(bestEnemyAIAction != null && enemyUnit.TrySpendActionPoints(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, completeAction);
            return true;
        }
        else
        {
            return false;
        }
    }
}
