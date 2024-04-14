using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestController : MonoBehaviour {

    public Quest currentQuest;
    private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;
    private bool isAllQuestCompleted = false;
    private DialogueController dialogueController; 
    private SpawnersController spawnersController; 
    private string questDescription, questInstructions;
    [SerializeField] private SceneController sceneController;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questMessage;
    [SerializeField] private TextMeshProUGUI questProgression;


    void Start() {
        DeactivateLight();
        this.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Interface/Backgrounds/quest_background_"+GameController.GetSystemLanguageLower());
    }

    void Update() {
        if(this.spawnersController == null){
            this.spawnersController = GameObject.Find("Spawners").GetComponent<SpawnersController>();
        }
        
        if(currentQuest.IsComplete() && dialogueController.GetDialogueInitiated())
            CompleteCurrentQuest();
    }

    public void InitQuestController() {
        this.dialogueController = GameObject.FindGameObjectWithTag("NPC").GetComponent<DialogueController>();
        InitializeQuests();   
        ShowCurrentQuest();
        this.dialogueController.ResetCurrentQuestIndex();
    }


    private void InitializeQuests() {
        IsAllQuestCompleted(false);

        if(this.sceneController.GetSceneName() == Names.Scenes[0]) {
            this.questDescription = GameController.GetSystemLanguageUpper() == "FR" ? "Parlez au prêtre" : "Talk to the priest";
            this.questInstructions = GameController.GetSystemLanguageUpper() == "FR" ? "Parlez au prêtre près du point d'apparition" : "Talk to the priest near the spawn point";
            quests.Add(new Quest(this.questDescription, this.questInstructions, 1, "Speaking")); 
        }
        else if(this.sceneController.GetSceneName() == Names.Scenes[1]) {
            this.questDescription = GameController.GetSystemLanguageUpper() == "FR" ? "Parlez au docteur" : "Talk to the doctor";
            this.questInstructions = GameController.GetSystemLanguageUpper() == "FR" ? "Parlez au docteur près du point d'apparition" : "Talk to the doctor near the spawn point";
            quests.Add(new Quest(this.questDescription, this.questInstructions, 1, "Speaking")); 
        }
        else /*if(this.sceneController.GetSceneName() == Names.Scenes[2])*/ {
            this.questDescription = GameController.GetSystemLanguageUpper() == "FR" ? "Parlez au fou" : "Talk to the madman";
            this.questInstructions = GameController.GetSystemLanguageUpper() == "FR" ? "Parlez à l'étrange individu près du point d'apparition" : "Talk to the weird guy near the spawn point";
            quests.Add(new Quest(this.questDescription, this.questInstructions, 1, "Speaking")); 
        }
    }

    void ShowCurrentQuest() {
        currentQuest = quests[currentQuestIndex];
        UpdateQuestText();
    }

    public void CompleteCurrentQuest() {
      
        currentQuest.SetCompleted(true);
        currentQuestIndex++;
        dialogueController.SetDialogueInitiated(false);

        if (currentQuestIndex < quests.Count) {
            ShowCurrentQuest();
            
            if(currentQuest.GetType() == "Finding"){
                Spawner[] spawners = spawnersController.GetComponentsInChildren<Spawner>();
                
                int required = currentQuest.GetRequired(); 

                List<int> selectedIndices = new List<int>();

                if (spawners != null && spawners.Length > 0) {
                    for (int i = 0; i < required; i++) {
                        int randomIndex;
                        do {
                            randomIndex = GameController.Random(0, spawners.Length -1 );
                        } while (selectedIndices.Contains(randomIndex)); 

                        selectedIndices.Add(randomIndex);

                        spawners[randomIndex].ActiveSpawnerPickUp();
                    }

                    foreach( Spawner spawner in spawners)
                        spawner.IncrementIndex();
                }
            }

        }
        else {

            string questDescription = GameController.GetSystemLanguageUpper() == "FR" ? "Changement de zone" : "Switch zones";
            string questInstructions = GameController.GetSystemLanguageUpper() == "FR" ? "Rendez-vous dans la zone suivante" : "Proceed to the next zone";
            quests.Add(new Quest(questDescription, questInstructions, 0, "None"));

            ActivateLight();
            ShowCurrentQuest();

            GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
            if (npcObject != null) {   
                dialogueController.SetIsInRange(false);
                npcObject.tag = "Untagged"; 
            }

            IsAllQuestCompleted(true);

        }
        UpdateQuestText();
    }

    public void UpdateQuestText() {
        Quest.QuestDetails questDetails = currentQuest.GetQuestDetails();
        string yellowTitle = questDetails.YellowTitle;
        string message = questDetails.Message;
        string progression = questDetails.Progression;


        questTitle.text = questDetails.YellowTitle;
        questMessage.text = questDetails.Message;
        questProgression.text = questDetails.Progression;
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest;
    }

    public void AddQuestFromDialogue(string questTitle, string questMessage, int requiredAmount, string questType) {
        quests.Add(new Quest(questTitle, questMessage, requiredAmount, questType));

		//! Apparemment les quêtes marchent plus si t'enlèves ça
	    Debug.Log("Quêtes actuelles :");

        foreach (Quest quest in quests) {
            Debug.Log($"- {quest.GetQuestDetails().YellowTitle}");
        }
    }


    public void IsAllQuestCompleted(bool state){
        isAllQuestCompleted = state;
    }

    public bool GetIsAllQuestCompleted(){
        return isAllQuestCompleted;
    }

    void ActivateLight() {
        GameObject lightObject = GameObject.FindGameObjectWithTag("LightNextZone");

        if (lightObject != null) {
            Light lightComponent = lightObject.GetComponent<Light>();
            lightComponent.enabled = true;
        }
        
    }

    void DeactivateLight()
    {
        GameObject lightObject = GameObject.FindGameObjectWithTag("LightNextZone");

        if (lightObject != null) {
            Light lightComponent = lightObject.GetComponent<Light>();
            lightComponent.enabled = false;
        }
    }
}
