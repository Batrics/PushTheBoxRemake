using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public string saveName;

    public void GameObjectSavingPosition() {
        Saving(transform.position);
    }
    private void Saving(Vector3 position) {
          PlayerPrefs.SetFloat(saveName + "X", position.x);
          PlayerPrefs.SetFloat(saveName + "Y", position.y);
          PlayerPrefs.SetFloat(saveName + "Z", position.z);
    }
}
