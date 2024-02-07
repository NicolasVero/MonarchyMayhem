using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour {
    
    [SerializeField] private bool isInRange = false;

    [Header("QuestRelation")]
    [SerializeField] private QuestController questController; // lié les 2 

    [Header("Canvas Settings")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Canvas interaction;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Canvas to Deactivate")]
    [SerializeField] private Canvas[] disabledCanvas;

    [Header("Dynamic Dialogue Settings")]
    private List<string> dialogueMessages = new List<string>();
    private int currentIndex;
    public bool dialogueInitiated = false; // Savoir si le dialogue a commencer
    private List<string> dynamicDialogue; // Liste des messages de dialogue dynamiques
    [SerializeField] private int messagesPerStep = 1; // Nombre de messages à afficher par étape
    [SerializeField] private List<string>[] dialogueSets; // Liste des ensembles de dialogues
    private int currentDialogueSetIndex = 0; // Indice de l'ensemble de dialogues actuel

    [Header("Quest Settings")]
    [SerializeField] private List<Quest> questList; // Liste des quêtes disponibles
    private int currentQuestIndex = 0; // Indice de la quête actuelle

    void Start() {
        GameController.SetCanvasVisibility(dialogueCanvas, false);

        dialogueSets = new List<string>[] {
            new List<string> { "Dialogue 1 - Message 1", "Dialogue 1 - Message 2", "Dialogue 1 - Message 3", "zejhgfezifgvze" },
            new List<string> { "Dialogue 2 - Message 1", "Dialogue 2 - Message 2", "Dialogue 2 - Message 3" },
            new List<string> { "Dialogue 3 - Message 1", "Dialogue 3 - Message 2", "Dialogue 3 - Message 3" },
            new List<string> { "Dialogue 4 - Message 1", "Dialogue 4 - Message 2", "Dialogue 4 - Message 3" },
            new List<string> { "Dialogue 5 - Message 1", "Dialogue 5 - Message 2", "Dialogue 5 - Message 3" },
            // Ajoutez autant de ensembles de dialogues que nécessaire
        };

        questList = new List<Quest> {
            new Quest("Quest 1", "Description de la quête 1", 1, "Finding"),
            new Quest("Quest 2", "Description de la quête 2", 2, "Finding"),
            new Quest("Quest 3", "Description de la quête 3", 1, "Speaking"),
            new Quest("Quest 4", "Description de la quête 4", 3, "Finding"),
            new Quest("Quest 5", "Va t en ", 3, "Speaking"),
            // Ajoutez autant de quêtes que nécessaire
        };    

        nextButton.onClick.AddListener(ShowNextMessage);
        prevButton.onClick.AddListener(ShowPreviousMessage);
        closeButton.onClick.AddListener(CloseDialogue);
        
    }

    void Update() {

        if(isInRange && Input.GetKeyDown(KeyCode.E))
            StartDialogue();

        Interaction();
    }

    private void Interaction() {
        interaction.enabled = isInRange && !dialogueCanvas.enabled;
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isInRange = false;
        }
    }

    private void ShowNextMessage() {
        this.currentIndex += messagesPerStep; 
        ShowMessage();

         if (this.currentIndex >= dynamicDialogue.Count) {
             CloseDialogue(); 
        }
    }


    private void ShowPreviousMessage() {
        this.currentIndex -= messagesPerStep; 
        if (this.currentIndex < 0)
            this.currentIndex = 0; 
        ShowMessage();
    }


    private void ShowMessage() {

        int endIndex = Mathf.Min(this.currentIndex + messagesPerStep, dynamicDialogue.Count); 
        dialogueText.text = ""; 
        for (int i = this.currentIndex; i < endIndex; i++) {
            dialogueText.text += dynamicDialogue[i] + "\n"; 
        }
    }

    private void CloseDialogue() {
        ManageDialogue(false);
    }

    private void StartDialogue() {

        this.currentIndex = 0; 
        ManageDialogue(true);
        SetCurrentDialogueSet(); 

        // Vérifiez si la quête actuelle est une quête de type "Speaking" pour démarrer une nouvelle quête
        if(questController.currentQuest.GetType() == "Speaking" && currentQuestIndex == 0){

            dialogueInitiated = true;
            SetCurrentDialogueSet();
            questController.AddQuestFromDialogue(questList[currentQuestIndex].GetTitle(), questList[currentQuestIndex].GetDescription(), questList[currentQuestIndex].GetRequired(), questList[currentQuestIndex].GetType());
        }
        if(questList[currentQuestIndex].GetType() == "Speaking"){ 
            currentDialogueSetIndex++;
            currentQuestIndex++;
            SetCurrentDialogueSet(); // Changer le dialogue en fonction de la nouvelle quête
            if(currentQuestIndex < questList.Count) {
                questController.AddQuestFromDialogue(questList[currentQuestIndex].GetTitle(), questList[currentQuestIndex].GetDescription(), questList[currentQuestIndex].GetRequired(), questList[currentQuestIndex].GetType());
            }
            dialogueInitiated = true;
        }
        if(dialogueInitiated == false && questController.currentQuest.IsComplete() ){ 
            currentQuestIndex++;
            currentDialogueSetIndex++;
            SetCurrentDialogueSet(); // Changer le dialogue en fonction de la nouvelle quête
            dialogueInitiated = true;
            if(currentQuestIndex < questList.Count) {
                questController.AddQuestFromDialogue(questList[currentQuestIndex].GetTitle(), questList[currentQuestIndex].GetDescription(), questList[currentQuestIndex].GetRequired(), questList[currentQuestIndex].GetType());
            }
        }
        ShowMessage(); 

    }

    private void SetCurrentDialogueSet() {
        if (currentDialogueSetIndex >= 0 && currentDialogueSetIndex < dialogueSets.Length) {
            dynamicDialogue = dialogueSets[currentDialogueSetIndex]; // Définir le nouvel ensemble de dialogues
        }
    }

    private void ManageDialogue(bool state) {
        dialogueCanvas.enabled = state;
        GameController.SetGameState(!state);

        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        for(int i = 0; i < disabledCanvas.Length; i++)
            disabledCanvas[i].enabled = !state;
    }

    public bool GetDialogueInitiated(){ //getter pour questcontroller
        return dialogueInitiated;
    }

    public void SetDialogueInitiated(bool state) {
        this.dialogueInitiated = state;
    }

    public bool GetIsInRangeFalse(){ //getter pour questcontroller
        return isInRange= false;
    }
}