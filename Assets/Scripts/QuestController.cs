using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class QuestController : MonoBehaviour
{
    public Quest currentQuest;
    private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;
    private int KillCounter;
    private bool isAllQuestCompleted = false;
    private DialogueController dialogueController; 
    private SpawnersController spawnersController; 

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questMessage;
    [SerializeField] private TextMeshProUGUI questProgression;



 
    void Start() {
        this.dialogueController = GameObject.FindGameObjectWithTag("NPC").GetComponent<DialogueController>();
        InitializeQuests();   
        ShowCurrentQuest();
    }

    void Update() {
        if(this.spawnersController == null){
            this.spawnersController = GameObject.Find("Spawners").GetComponent<SpawnersController>();
        }
        
        if(currentQuest.IsComplete() && dialogueController.GetDialogueInitiated()) {
            Debug.Log("dedans");
            CompleteCurrentQuest();
        } else {
            Debug.Log("Dehors");
        }
    }


    void InitializeQuests()
    {
        IsAllQuestCompleted(false);
        quests.Add(new Quest("Parler au prêtre", "Parler au prêtre près du point d'apparition", 1, "Speaking"));   
    }

    void ShowCurrentQuest()
    {
        currentQuest = quests[currentQuestIndex];
        UpdateQuestText();
    }

    public void CompleteCurrentQuest() {
    
        currentQuest.SetCompleted(true);
        currentQuestIndex++;
        dialogueController.SetDialogueInitiated(false);


        if(currentQuestIndex < quests.Count) {
            ShowCurrentQuest();
            
            if(currentQuest.GetType() == "Finding")
                SpawnPickUps();
        } else {

            GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
            dialogueController.SetIsInRange(false);
            npcObject.tag = "Untagged"; 
            IsAllQuestCompleted(true);
        }

        UpdateQuestText();
    }

    private void SpawnPickUps() {
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

    public void UpdateQuestText()
    {
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

    }


    public void IsAllQuestCompleted(bool state){
        isAllQuestCompleted = state;
    }

    public bool GetIsAllQuestCompleted(){
        return isAllQuestCompleted;
    }
}