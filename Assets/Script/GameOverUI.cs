using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public int a = 0;
    public void GameOverButton()
    {
        SceneManager.LoadScene(a);
    }

}
