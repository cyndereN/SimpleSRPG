using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    [SerializeField]
    private Unit unit;
    [SerializeField]
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UASOnSelect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UASOnSelect(object sender,EventArgs empty)
    {
        if(UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UASOnSelect;
    }

}
