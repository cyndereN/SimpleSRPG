using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField]private GameObject gO;
    [SerializeField]private float destroyTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gO, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
