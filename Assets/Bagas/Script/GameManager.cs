using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // public List<Transform> levelGo = new List<Transform>();
    public Transform player;
    public AudioManager audioManager;
    public int level;

    private void Start() {
        LoadPlayerPos();
    }

    //Back to Main Menu
    public void ResetSavePoint() {
        SaveManager saveManagerPlayer = player.GetComponent<SaveManager>();
        SaveManager saveManagerTimer = GameObject.Find("Timer").GetComponent<SaveManager>();

        PlayerPrefs.DeleteKey(saveManagerPlayer.saveName + "X");
        PlayerPrefs.DeleteKey(saveManagerPlayer.saveName + "Y");
        PlayerPrefs.DeleteKey(saveManagerPlayer.saveName + "Z");
        PlayerPrefs.DeleteKey(saveManagerTimer.saveName);
    }

    // private void LoadPos(int level) {
    //     SceneManager.LoadScene("Level Design");
    //     Transform boxParent = levelGo[level-1].Find("Box");
    //     List<Transform> boxs = new List<Transform>();
    //     for(int i = 0; i < boxParent.childCount; i++) {
    //         boxs.Add(boxParent.GetChild(i).transform);
    //         LoadGameFunc(boxs[i], level + "-" + (i+1));
    //     }
    // }
    private void LoadPlayerPos() {
        SaveManager saveManagerPlayer = player.GetComponent<SaveManager>();
        if(PlayerPrefs.HasKey(saveManagerPlayer.saveName + "X") && PlayerPrefs.HasKey(saveManagerPlayer.saveName + "Y") && PlayerPrefs.HasKey(saveManagerPlayer.saveName + "Z")) {
            LoadGameFunc(player, saveManagerPlayer.saveName);
            print("X : " + PlayerPrefs.GetFloat(saveManagerPlayer.saveName + "X"));
            print("Y : " + PlayerPrefs.GetFloat(saveManagerPlayer.saveName + "Y"));
            print("Z : " + PlayerPrefs.GetFloat(saveManagerPlayer.saveName + "Z"));
        }
        else {
            print("SavePoint = null");
        }
    }

    //Restart click R
    public void Restart() {
        SceneManager.LoadScene("Level Design");
    }

    private void LoadGameFunc(Transform transformGo, string saveName) {
        float x = PlayerPrefs.GetFloat(saveName + "X");
        float y = PlayerPrefs.GetFloat(saveName + "Y");
        float z = PlayerPrefs.GetFloat(saveName + "Z");
        
        transformGo.position = new Vector3(x, y, z);
    }
}
