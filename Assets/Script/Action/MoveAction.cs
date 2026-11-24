using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class MoveAction : BaseAction
{
    [Header("Movement")]
    public float moveSpeed = 4.0f;
    public float rotatonSpeed = 0.5f;
    private bool isMoving = false;


    [Header("Distance")]
    [SerializeField]private int maxMoveDistance = 1;
    private Vector3 targetPosition;
    private float stopDistance = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isMoving) return;

        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position = transform.position + moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotatonSpeed);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("IsWalking", false);
            ActionEnd();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isMoving = true;
        this.onActionComplete = onActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offectGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offectGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxMoveDistance)
                {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "移动";
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override void ActionEnd()
    {
        onActionComplete();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int target = unit.GetAttackAction().GetTargetCountAtPosition(gridPosition);

        if (unit.GetAttackAction().GetTargetCountAtPosition(unit.GetGridPosition())>0)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = -100,
            };
        }
        else if(target > 0)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 10 * target,
            };
        }
        else
        {
            List<Unit> playerList = UnitManger.Instance.GetFriendUnitList();
            float Maxdistance = 0;
            foreach(Unit player in playerList)
            {
                if(player.GetGridPosition()==gridPosition)
                {
                    continue;
                }
                float distance = Vector2.Distance(new Vector2(gridPosition.x, gridPosition.z),
                    new Vector2(player.GetGridPosition().x, player.GetGridPosition().z));
                if (distance > Maxdistance)
                {
                    Maxdistance = distance;
                }
            }
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = (int)(10 * Maxdistance),
            };
        }
    }
}
