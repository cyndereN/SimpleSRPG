using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;

    [SerializeField]private Text text;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gridObject.ToString();
    }
}
