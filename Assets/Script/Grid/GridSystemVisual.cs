using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private GameObject gridSPrefab;

    [Serializable]
    public struct GrideVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft,
    }

    [SerializeField]private List<GrideVisualTypeMaterial> grideVisualTypeMaterials;

    public static GridSystemVisual Instance { get; private set; }

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridSystemVisual in the scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gridSystemVisualSingles = new GridSystemVisualSingle
            [LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        LevelGrid.Instance.OnAnyUnitMoved += LGOnAnyUnitMoved;
        UnitActionSystem.Instance.OnSelectedActionChanged += UAOnSelectedActionChanged;

        for (int x = 0;x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0;z< LevelGrid.Instance.GetHeight();z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject gP =
                Instantiate(gridSPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingles[x,z] = gP.GetComponent<GridSystemVisualSingle>();   
            }
        }

        UpdataGridVisual();
    }

    private void UAOnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdataGridVisual();
    }

    private void LGOnAnyUnitMoved(object sender, EventArgs e)
    {
        UpdataGridVisual();
    }



    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingles[x, z].Hide();
            }
        }
    }

    public void ShowGridPosition(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].
                Show(GetMaterialFromGridVisualType(gridVisualType));
        }
    }

    private void UpdataGridVisual()
    {
        HideAllGridPosition();
        GridVisualType gridVisualType;
        Unit selectUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        if(selectedAction)
        {
            switch(selectedAction)
            {
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case AttackAction attackAction:
                    gridVisualType = GridVisualType.Red;
                    ShowGridPositionRange(selectUnit.GetGridPosition(), attackAction.GetAttackRange());
                    break;
                case BideAction bideAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
                case SkillAction skillAction:
                    gridVisualType = GridVisualType.Yellow;
                    ShowGridPositionRange(selectUnit.GetGridPosition(), skillAction.GetAttackRange());
                    break;
                default:
                    gridVisualType = GridVisualType.Yellow;
                    break;

            }
            ShowGridPosition(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        }
        
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range)
    {
        List<GridPosition>gridPositionList = new List<GridPosition>();
        for(int x = -range; x<= range; x++)
        {
            for(int z = -range; z <= range; z++)
            {
                GridPosition grid = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(grid))
                {
                    continue;
                }

                if (gridPosition == grid)
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(grid);
            }
        }
        ShowGridPosition(gridPositionList, GridVisualType.RedSoft);
    }

    private Material GetMaterialFromGridVisualType(GridVisualType gridVisualType)
    {
        foreach(GrideVisualTypeMaterial grideVisualTypeMaterial in grideVisualTypeMaterials)
        {
            if(gridVisualType == grideVisualTypeMaterial.gridVisualType)
            {
                return grideVisualTypeMaterial.material;
            }
        }
        return null;
    }

}
