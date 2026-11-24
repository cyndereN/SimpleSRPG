using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManger : MonoBehaviour
{
    public static GameOverManger Instance { get; private set; }
    private bool isGameOver = false;
    [SerializeField]private GameObject gameoverUI;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one SoundManger in the scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(UnitManger.Instance.GetFriendUnitList().Count<=0 ||
        UnitManger.Instance.GetEnemyUnitList().Count <= 0)
        {
            isGameOver = true;
            gameoverUI.SetActive(true);
        }
    }

}
