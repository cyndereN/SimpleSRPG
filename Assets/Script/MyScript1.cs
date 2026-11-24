using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyScript1 : MonoBehaviour
{
    public int a = 10;
    public float b = 3.14f;
    public bool c = true;
    public string d = "text";
    public Vector3 e = new Vector3(1,2,3);
    public Text text1;
    public GameObject gameObject1;

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}




//这是注释
//Debug.Log("Hello World");打印事件
//text1.text = d; 设置值
//gameObject1.transform.Translate(e * Time.deltaTime); 移动游戏物体
//gameObject1.transform.Rotate(e); 移动游戏物体
//[SerializeField] 序列化