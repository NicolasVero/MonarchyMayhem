using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    public Quest currentQuest;
    private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;
    private int KillCounter;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questMessage;
    [SerializeField] private TextMeshProUGUI questProgression;
    [SerializeField] private DialogueController dialogueController; // lié l'un vers l'autre



 
    void Start() {
        InitializeQuests();   
        ShowCurrentQuest();
    }

    void Update() {
        CompleteCurrentQuest();
    }


    void InitializeQuests()
    {
        quests.Add(new Quest("Parler au prêtre", "Parler au prêtre près du point d'apparition", 1, "Speaking"));   
    }

    void ShowCurrentQuest()
    {
        currentQuest = quests[currentQuestIndex];
        UpdateQuestText();
    }

    public void CompleteCurrentQuest() {

        if(currentQuest.IsComplete() && dialogueController.GetDialogueInitiated()) { // permet de parler au pnj pour pouvoir recevoir une quete avant de passer a la quete d'apres sinon sa bug

            currentQuest.SetCompleted(true);
            currentQuestIndex++;
            dialogueController.SetDialogueState(false); // remet a false pour pouvoir etre relancer

            if (currentQuestIndex < quests.Count)
            {
                ShowCurrentQuest();
                Debug.Log(currentQuestIndex);
            }
            else
            {
                Debug.Log("Toutes les quêtes ont été complétées. Passage au niveau suivant...");
                GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
                if (npcObject != null)
                {   
                    dialogueController.SetIsInRange(false);
                    npcObject.tag = "Untagged";
                }
            }
        }

        UpdateQuestText();
    }

    void UpdateQuestText()
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

        Debug.Log("Quêtes actuelles :");

        foreach (Quest quest in quests) {
            Debug.Log($"- {quest.GetQuestDetails().YellowTitle}");
        }
    }
}