using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public GameObject cube;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetPosition()==Vector3.zero)
        {
            cube.SetActive(false);
        }
        else
        {
            cube.SetActive(true);
            cube.transform.position = GetPosition();
        }
        
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray,out RaycastHit hit,float.MaxValue, LayerMask.GetMask("GamePlane"));
        return hit.point;
    } 

}
