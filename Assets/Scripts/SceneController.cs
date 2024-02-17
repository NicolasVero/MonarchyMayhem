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

    void Awake() {

        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.playerController.transform.position = SetSpawnPoint();

        if (this.GetSceneName() != "Salle_combat_final"){
            this.interactionScreen = GameObject.Find("Interaction").GetComponent<Canvas>();
            this.pickUp = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponentInChildren<PickUp>();
            this.questController = GameObject.Find("Quest Menu").GetComponent<QuestController>();
            this.questController.InitQuestController();
            this.pickUp.InitPickUp();
            this.playerController.ConfigureQuestCanvas();
        }
    }

    void Update() {
        if(this.isInRange && Input.GetKeyDown(KeyCode.E)){
            ChooseNextZone(this.GetSceneName());
        }
        this.ShowInteraction();
    }

    private void ShowInteraction() {
        if (this.GetSceneName() != "Salle_combat_final")
            this.interactionScreen.enabled = this.isInRange;
    }

    private void ChooseNextZone(string sceneName) {
        if(!questController.GetIsAllQuestCompleted())
            return;

        string[] sceneNames = {"Tutorial", "Village", "Chateau", "Salle_combat_final"};
        int index = Array.IndexOf(sceneNames, sceneName);
        if (index >= 0)
            ChangeScene(sceneNames[(index + 1)%sceneNames.Length]);
    }
    
    private void ChangeScene(string nextSceneName) {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other) {
        // if(other.CompareTag("Player")) {
            // if(questController.GetIsAllQuestCompleted())
                this.isInRange = true;
        // }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            this.isInRange = false;
        }
    }

    public Vector3 SetSpawnPoint() { return GameObject.FindGameObjectWithTag("SpawnPoint").transform.position; }
    public string GetSceneName() { return SceneManager.GetActiveScene().name; }

}
