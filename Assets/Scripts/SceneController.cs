using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private bool isInRange = false;
    private PlayerController playerController;
    private QuestController questController;
    private Canvas interactionScreen;
    private PickUp pickUp;
    private DanceWheelController danceWheelController;

    void Awake() {

        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.playerController.transform.position = SetSpawnPoint();
        this.playerController.ConfigureQuestCanvas();

        if(this.GetSceneName() != Names.Scenes[3]) {

            this.playerController.AddWeaponsDropper();
            this.danceWheelController = GameObject.Find("DanceWheel").GetComponent<DanceWheelController>();
            this.danceWheelController.InitQuestController();
            this.playerController.InitSceneController();
            this.interactionScreen = GameObject.Find("Interaction").GetComponent<Canvas>();
            this.pickUp = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponentInChildren<PickUp>();
            this.questController = GameObject.Find("Quest Menu").GetComponent<QuestController>();
            this.questController.InitQuestController();
            this.pickUp.InitPickUp();
            this.playerController.ConfigureQuestCanvas();

            GameController.SetCanvasVisibility(this.interactionScreen, false);
        } else {
            this.playerController.InitBossCanvas();
        }
    }

    void Update() {
        if(this.isInRange && Input.GetKeyDown(KeyCode.E)){
            ChooseNextZone(this.GetSceneName());
        }

        if(this.playerController.transform.position.y <= -10)
            this.playerController.transform.position = SetSpawnPoint();
    }

    private void ChooseNextZone(string sceneName) {
        // if(!questController.GetIsAllQuestCompleted())
        //     return;

        string[] sceneNames = Names.Scenes;
        int index = Array.IndexOf(sceneNames, sceneName);
        if (index >= 0)
            ChangeScene(sceneNames[(index + 1) % sceneNames.Length]);
    }
    
    private void ChangeScene(string nextSceneName) {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            // if(questController.GetIsAllQuestCompleted()) {
                GameController.SetCanvasVisibility(this.interactionScreen, true);
                GameController.SetPanelVisibility(this.interactionScreen.gameObject, true);
                this.isInRange = true;
            // }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            GameController.SetCanvasVisibility(this.interactionScreen, false);
            GameController.SetPanelVisibility(this.interactionScreen.gameObject, false);
            this.isInRange = false;
        }
    }
    
    public Vector3 SetSpawnPoint() { 
        return GameObject.FindGameObjectWithTag("SpawnPoint").transform.position; 
    }

    public string GetSceneName() { 
        return SceneManager.GetActiveScene().name; 
    }
}
