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
    [SerializeField] private SceneController sceneController;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questMessage;
    [SerializeField] private TextMeshProUGUI questProgression;


    void Start() {}

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


    void InitializeQuests() {
        IsAllQuestCompleted(false);


        switch (this.sceneController.GetSceneName()){
            case "Tutorial":
                quests.Add(new Quest("Parlez au prêtre", "Parlez au prêtre près du point d'apparition", 1, "Speaking")); 
                break;
            case "Village":
                quests.Add(new Quest("Parlez au docteur", "Parlez au docteur près du point d'apparition", 1, "Speaking")); 
                break;
            case "Chateau":
                quests.Add(new Quest("Parlez au fou", "Parlez à l'étrange individu dans le château", 1, "Speaking")); 
                break;
        }  
    }

    void ShowCurrentQuest()
    {
        currentQuest = quests[currentQuestIndex];
        UpdateQuestText();
    }

    public void CompleteCurrentQuest() {

        // if(currentQuest.IsComplete() && dialogueController.GetDialogueInitiated()) { // permet de parler au pnj pour pouvoir recevoir une quete avant de passer a la quete d'apres sinon sa bug
                
            currentQuest.SetCompleted(true);
            currentQuestIndex++;
            dialogueController.SetDialogueInitiated(false); // remet a false pour pouvoir etre relancer

            if (currentQuestIndex < quests.Count)
            {
                ShowCurrentQuest();
                
                if(currentQuest.GetType() == "Finding"){
                    Spawner[] spawners = spawnersController.GetComponentsInChildren<Spawner>();
                    
                    int required = currentQuest.GetRequired(); 

                    List<int> selectedIndices = new List<int>();

                    if (spawners != null && spawners.Length > 0)
                    {
                        for (int i = 0; i < required; i++)
                        {
                            int randomIndex;
                            do
                            {
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
            else
            {
                Debug.Log("Toutes les quêtes ont été complétées. Passage au niveau suivant...");
                quests.Add(new Quest("Changement de zone", "Rendez-vous dans la zone suivante", 0, "Speaking"));
                ShowCurrentQuest();

                GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
                if (npcObject != null)
                {   
                    dialogueController.GetIsInRangeFalse();
                    npcObject.tag = "Untagged"; 
                }

                IsAllQuestCompleted(true);

            }
        // }

        UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        Quest.QuestDetails questDetails = currentQuest.GetQuestDetails();
        string yellowTitle = questDetails.YellowTitle;
        string message = questDetails.Message;
        string progression = questDetails.Progression;


        questTitle.text = questDetails.YellowTitle;
        questMessage.text = questDetails.Message;
        questProgression.text = questDetails.Progression;
         Debug.Log("UpdateQuestText");
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest;
    }

    public void AddQuestFromDialogue(string questTitle, string questMessage, int requiredAmount, string questType) {

        quests.Add(new Quest(questTitle, questMessage, requiredAmount, questType));

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
}