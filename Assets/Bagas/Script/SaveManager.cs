using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string saveName;

    //after cutscene Level
    public void GameObjectSavingPosition() {
        SavingPos(transform.position);
    }
    private void SavingPos(Vector3 position) {
        PlayerPrefs.SetFloat(saveName + "X", position.x);
        PlayerPrefs.SetFloat(saveName + "Y", position.y);
        PlayerPrefs.SetFloat(saveName + "Z", position.z);
    }
    public void SavingFloat(float num) {
        PlayerPrefs.SetFloat(saveName, num);
    }
}
