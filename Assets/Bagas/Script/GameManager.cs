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
        Transform boxParent = levelGo[level].Find("Box");
        List<Transform> boxs = new List<Transform>();
        for (int i = 0; i < boxParent.childCount; i++) {
            SaveManager SaveManager = boxParent.GetChild(i).GetComponent<SaveManager>();
            BoxScript2 boxScript2 = boxParent.GetChild(i).GetComponent<BoxScript2>();
            boxs.Add(boxParent.GetChild(i).transform);
            LoadGameFunc(boxs[i], SaveManager.boxFirstPos);
            boxScript2.targetPos = SaveManager.boxFirstPos;
        }
    }

    private bool LoadPlayerPos()
    {
        // SaveManagerPlayer saveManagerPlayer = player.GetComponent<SaveManagerPlayer>();
        PlayerMovement2 playerMovement2 = player.GetComponent<PlayerMovement2>();
        LoadGameFunc(player, playerMovement2.firstPosPoint);
        Debug.Log("Player position loaded from save data.");
        return true;
    }

    // Restart click R
    public void Restart()
    {
        Debug.Log("restarttt");
        SetPlayerToStartPosition(level);
        LoadPos(level);
    }

    private void SetPlayerToStartPosition(int level)
    {
        Transform playerStartPos = levelGo[level].Find("PlayerStartPos");
        if (playerStartPos != null)
        {
            player.position = playerStartPos.position;
            Debug.Log("Player position set to start position.");
        }
        else
        {
            Debug.LogWarning("PlayerStartPos not found in level " + level);
        }
    }

    private void LoadGameFunc(Transform transformGo, Vector3 savePos)
    {
        transformGo.position = savePos;
    }

    // Move to the next level
    public void NextLevel()
    {
        Debug.Log("its on nextlevel function");
        levelGo[level].gameObject.SetActive(false);
        level++; // Increment the level counter
        if (level == 3)
        {
            SceneManager.LoadScene("Main Menu");
        }
        if (level <= levelGo.Count)
        {
            // LoadPos(level); // Load saved positions for objects in the new level

            // Find the player's start position in the new level
            Transform playerStartPos = levelGo[level].Find("PlayerStartPos");
            levelGo[level].gameObject.SetActive(true);
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

    public void NewRestart() {

    }
}
