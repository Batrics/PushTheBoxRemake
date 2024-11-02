using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Transform> levelGo = new List<Transform>();
    public Transform player;
    public AudioManager audioManager;
    public int level;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

    }

    private void Start()
    {
        // LoadPlayerPos();
    }

    // Back to Main Menu
    public void ResetSavePoint()
    {
        SaveManagerPlayer saveManagerPlayer = player.GetComponent<SaveManagerPlayer>();

        PlayerPrefs.DeleteKey(saveManagerPlayer.saveName + "X");
        PlayerPrefs.DeleteKey(saveManagerPlayer.saveName + "Y");
        PlayerPrefs.DeleteKey(saveManagerPlayer.saveName + "Z");
    }

    private void LoadPos(int level)
    {
        // SceneManager.LoadScene("Level Design");
        Transform boxParent = levelGo[level - 1].Find("Box");
        List<Transform> boxs = new List<Transform>();
        for (int i = 0; i < boxParent.childCount; i++)
        {
            boxs.Add(boxParent.GetChild(i).transform);
            LoadGameFunc(boxs[i], level + "-" + (i + 1));
        }
    }

    private void LoadPlayerPos()
    {
        SaveManagerPlayer saveManagerPlayer = player.GetComponent<SaveManagerPlayer>();
        if (PlayerPrefs.HasKey(saveManagerPlayer.saveName + "X") && PlayerPrefs.HasKey(saveManagerPlayer.saveName + "Y") && PlayerPrefs.HasKey(saveManagerPlayer.saveName + "Z"))
        {
            LoadGameFunc(player, saveManagerPlayer.saveName);
            print("X : " + PlayerPrefs.GetFloat(saveManagerPlayer.saveName + "X"));
            print("Y : " + PlayerPrefs.GetFloat(saveManagerPlayer.saveName + "Y"));
            print("Z : " + PlayerPrefs.GetFloat(saveManagerPlayer.saveName + "Z"));
        }
        else
        {
            print("SavePoint = null");
        }
    }

    // Restart click R
    public void Restart()
    {
        LoadPlayerPos();
        LoadPos(level);
    }

    private void LoadGameFunc(Transform transformGo, string saveName)
    {
        float x = PlayerPrefs.GetFloat(saveName + "X");
        float y = PlayerPrefs.GetFloat(saveName + "Y");
        float z = PlayerPrefs.GetFloat(saveName + "Z");

        transformGo.position = new Vector3(x, y, z);
    }

    // Move to the next level
    public void NextLevel()
    {
        
        Debug.Log("its on nextlevel function");
        level++; // Increment the level counter
        if (level <= levelGo.Count)
        {
            LoadPos(level); // Load saved positions for objects in the new level

            // Find the player's start position in the new level
            Transform playerStartPos = levelGo[level - 1].Find("PlayerStartPos");
            if (playerStartPos != null)
            {
                player.position = playerStartPos.position; // Set the player's position to the start position
            }
            else
            {
                Debug.LogWarning("PlayerStartPos not found in level " + level);
            }
        }
        else
        {
            Debug.LogWarning("No more levels available.");
        }
    }

    public void triggerWin()
    {
        WinUIManager.instance.triggerWin();
    }
}
