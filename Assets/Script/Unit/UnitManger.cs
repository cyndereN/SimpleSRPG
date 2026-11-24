using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManger : MonoBehaviour
{
    public static UnitManger Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> friendUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitManger in the scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    void Start()
    {
        Unit.OnAnyUnitSpawned += UOnAnyUnitSpawned;
        Unit.OnAnyUnitDead += UOnAnyUnitDead;
    }

    private void UOnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendUnitList.Add(unit);
        }
    }

    private void UOnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendUnitList()
    {
        return friendUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
    


}
