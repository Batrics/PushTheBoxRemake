using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIManager : MonoBehaviour
{
    public static WinUIManager instance;
    [SerializeField] private GameObject content;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void triggerWin()
    {
       content.SetActive(true);
    }

    public void nextLevel()
    {
        GameManager.Instance.NextLevel();
    }

    public void restart()
    {
        GameManager.Instance.Restart();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
