using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(MainCamera.transform);
    }
}
