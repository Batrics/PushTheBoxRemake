using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsTriggerScript : MonoBehaviour
{
    public int level;
    public GameObject thisCuts;
    public GameObject nextCuts;
    public PlayableDirector director;
    private GameManager gameManager;
    private void OnCutsceneFinished(PlayableDirector pd){
        SaveManagerPlayer saveManagerPlayer = GameObject.Find("Player").GetComponent<SaveManagerPlayer>();
        saveManagerPlayer.GameObjectSavingPosition();
        Debug.Log("Cutscene selesai!");
    }
    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Cuts")) {
            director = nextCuts.GetComponent<PlayableDirector>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.level = level;
            PlayDirector(director);
            print("A");
        }
    }

    private void PlayDirector(PlayableDirector pd){
        pd.stopped += OnCutsceneFinished;
        pd.Play();
    }

}
