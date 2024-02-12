using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour {

    private QuestController questController; 
    [SerializeField] private NPCController npcController;

    [Header("Canvas Settings")]
    [SerializeField]private Canvas dialogueCanvas;
    [SerializeField]private Canvas interaction;
    [SerializeField]private TextMeshProUGUI dialogueText;
    [SerializeField]private Button closeButton;
    [SerializeField]private Button nextButton;
    [SerializeField]private Button prevButton;

    [Header("Canvas to Deactivate")]
    [SerializeField]private Canvas[] disabledCanvas;

    [Header("Dynamic Dialogue Settings")]
    [SerializeField] private int messagesPerStep = 1; // Nombre de messages à afficher par étape
    [SerializeField] private bool isInRange = false;

    private List<string> dialogueMessages = new List<string>();
    private List<string> dynamicDialogue;
    private List<string>[] dialogueSets; // Liste des ensembles de dialogues
    private List<Quest> questList; // Liste des quêtes disponibles

    private int currentIndex;
    private int currentDialogueSetIndex = 0; // Indice de l'ensemble de dialogues actuel
    private int currentQuestIndex = 0; // Indice de la quête actuelle
    
    public bool dialogueInitiated = false; // Savoir si le dialogue a commencer
   
    private GameObject npcObject;

   void Start() {

    this.questController = GameObject.Find("Quest Menu").GetComponent<QuestController>();
    this.dialogueCanvas = GameObject.Find("Dialogue").GetComponent<Canvas>();
    this.interaction = GameObject.Find("Interaction").GetComponent<Canvas>();

    // Si closeButton est null, vous pouvez essayer de le trouver
    if (closeButton == null)
    {
        // Recherchez le bouton Close dans le parent dialogueParent
        if (dialogueCanvas != null)
        {   
            Transform dialogueTextTransform = dialogueCanvas.transform.Find("NPC_Dialogue");
            Transform closeButtonTransform = dialogueCanvas.transform.Find("Buttons/Close");
            Transform previousTransform = dialogueCanvas.transform.Find("Buttons/Previous");
            Transform nextTransform = dialogueCanvas.transform.Find("Buttons/Next");
            
            if (closeButtonTransform != null && previousTransform != null && nextTransform != null && dialogueTextTransform != null)
            {
                this.closeButton = closeButtonTransform.GetComponent<Button>();
                this.prevButton = previousTransform.GetComponent<Button>();
                this.nextButton = nextTransform.GetComponent<Button>();
                this.dialogueText = dialogueTextTransform.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("Le bouton de fermeture n'a pas été trouvé dans le parent 'Buttons'.");
            }
        }
        else
        {
            Debug.LogError("Le parent 'Dialogue' n'a pas été assigné dans l'inspecteur Unity.");
        }
    }
    else
    {
        // closeButton est déjà défini dans l'inspecteur Unity, vous n'avez pas besoin de le trouver
        closeButton.onClick.AddListener(CloseDialogue);
    }

    List<Canvas> combinedCanvasList = new List<Canvas>();

    Canvas questMenuCanvas = GameObject.Find("Quest Menu").GetComponent<Canvas>();
    if (questMenuCanvas != null)
    {
        combinedCanvasList.Add(questMenuCanvas);
    }
    else
    {
        Debug.LogError("Le canvas 'Quest Menu' n'a pas été trouvé.");
    }

    Canvas hudCanvas = GameObject.Find("HUD").GetComponent<Canvas>();
    if (hudCanvas != null)
    {
        combinedCanvasList.Add(hudCanvas);
    }
    else
    {
        Debug.LogError("Le canvas 'HUD' n'a pas été trouvé.");
    }

    // Convertir la liste en tableau si nécessaire
    disabledCanvas = combinedCanvasList.ToArray();

    GameController.SetCanvasVisibility(dialogueCanvas, false);

    dialogueSets = npcController.GetDialogueSets();
    questList = npcController.GetQuestList();

    // dialogueSets = new List<string>[] {
    //     new List<string> { "Dialogue 1 - Message 1", "Dialogue 1 - Message 2", "Dialogue 1 - Message 3", "zejhgfezifgvze" },
    //     new List<string> { "Dialogue 2 - Message 1", "Dialogue 2 - Message 2", "Dialogue 2 - Message 3" },
    //     new List<string> { "Dialogue 3 - Message 1", "Dialogue 3 - Message 2", "Dialogue 3 - Message 3" },
    //     new List<string> { "Dialogue 4 - Message 1", "Dialogue 4 - Message 2", "Dialogue 4 - Message 3" },
    //     new List<string> { "Dialogue 5 - Message 1", "Dialogue 5 - Message 2", "Dialogue 5 - Message 3" },
    //     // Ajoutez autant de ensembles de dialogues que nécessaire
    // };

    // questList = new List<Quest> {
    //     new Quest("Quest 1", "Description de la quête 1", 1, "Finding"),
    //     new Quest("Quest 2", "Description de la quête 2", 2, "Finding"),
    //     new Quest("Quest 3", "Description de la quête 3", 1, "Speaking"),
    //     new Quest("Quest 4", "Description de la quête 4", 3, "Finding"),
    //     new Quest("Quest 5", "Va t en ", 3, "Speaking"),
    //     // Ajoutez autant de quêtes que nécessaire
    // };    

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
        GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
        if (other.CompareTag("Player") && npcObject != null && npcObject.CompareTag("NPC")) //! Changer
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
        if (other.CompareTag("Player") && npcObject != null && npcObject.CompareTag("NPC")) //! Changer
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