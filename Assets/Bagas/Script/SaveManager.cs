using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string saveName;

    //after cutscene Level
    private void Start() {
        GameObjectSavingPosition();
    }
    public void GameObjectSavingPosition() {
        BoxScript2 BoxScript2 = gameObject.GetComponent<BoxScript2>();
        SavingPos(BoxScript2.boxFirstPos);
        PlayerPrefs.Save();
    }
    private void SavingPos(Vector3 position) {
        PlayerPrefs.SetFloat(saveName + "X", position.x);
        PlayerPrefs.SetFloat(saveName + "Y", position.y);
        PlayerPrefs.SetFloat(saveName + "Z", position.z);
        print(position);
    }
    public void SavingFloat(float num) {
        PlayerPrefs.SetFloat(saveName, num);
    }
}
