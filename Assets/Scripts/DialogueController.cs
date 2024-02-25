using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour {

    private QuestController questController; 
    [SerializeField] private NPCController npcController;

    private Canvas dialogueCanvas;
    private Canvas interaction;
    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI nameText;
    private Image picture;
    private Button closeButton;
    private Button nextButton;
    private Button prevButton;

    [Header("Canvas to Deactivate")]
    [SerializeField] private Canvas[] disabledCanvas;

    [Header("Dynamic Dialogue Settings")]
    [SerializeField] private int messagesPerStep = 1; 
    [SerializeField] private bool isInRange = false;

    private List<string> dialogueMessages = new List<string>();
    private List<string> dynamicDialogue;
    private List<string>[] dialogueSets;
    private List<Quest> questList; 

    private int currentIndex;
    private int currentDialogueSetIndex = 0; 
    private int currentQuestIndex = 0; 
    
    public bool dialogueInitiated = false;
   
    private GameObject npcObject;

    void Start() {
        
        this.currentDialogueSetIndex = 0;
        this.questController = GameObject.Find("Quest Menu").GetComponent<QuestController>();
        this.dialogueCanvas = GameObject.Find("Dialogue").GetComponent<Canvas>();
        this.interaction = GameObject.Find("Interaction").GetComponent<Canvas>();

        Transform dialogueTextTransform = dialogueCanvas.transform.Find("NPC_Dialogue");
        Transform nameTextTransform = dialogueCanvas.transform.Find("NPC_Name");
        Transform pictureTextTransform = dialogueCanvas.transform.Find("NPC_Picture");
        Transform closeButtonTransform = dialogueCanvas.transform.Find("Buttons/Close");
        Transform previousTransform = dialogueCanvas.transform.Find("Buttons/Previous");
        Transform nextTransform = dialogueCanvas.transform.Find("Buttons/Next");
        
        if (closeButtonTransform != null && previousTransform != null && nextTransform != null && dialogueTextTransform != null && nameTextTransform != null) {
            this.closeButton = closeButtonTransform.GetComponent<Button>();
            this.prevButton = previousTransform.GetComponent<Button>();
            this.nextButton = nextTransform.GetComponent<Button>();
            this.dialogueText = dialogueTextTransform.GetComponent<TextMeshProUGUI>();
            this.nameText = nameTextTransform.GetComponent<TextMeshProUGUI>();
            this.picture = pictureTextTransform.GetComponent<Image>();
        }


        List<Canvas> combinedCanvasList = new List<Canvas>();
        Canvas questMenuCanvas = GameObject.Find("Quest Menu").GetComponent<Canvas>();
        Canvas hudCanvas = GameObject.Find("HUD").GetComponent<Canvas>();

        combinedCanvasList.Add(questMenuCanvas);
        combinedCanvasList.Add(hudCanvas);
        combinedCanvasList.Add(this.interaction);
        this.disabledCanvas = combinedCanvasList.ToArray();

        this.npcObject = GameObject.FindGameObjectWithTag("NPC");

        GameController.SetCanvasVisibility(dialogueCanvas, false);


        this.nextButton.onClick.AddListener(ShowNextMessage);
        this.prevButton.onClick.AddListener(ShowPreviousMessage);
        this.closeButton.onClick.AddListener(CloseDialogue);

        this.npcController = this.npcObject.GetComponent<NPCController>();

        this.dialogueSets = this.npcController.GetDialogueSets();
        this.questList = this.npcController.GetQuestList();
    }

    void Update() {

        if(this.isInRange && Input.GetKeyDown(KeyCode.E))
            this.StartDialogue();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && this.npcObject != null && this.npcObject.CompareTag("NPC")) {
            GameController.SetCanvasVisibility(this.interaction, !this.dialogueCanvas.enabled);
            this.isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player") && this.npcObject != null && this.npcObject.CompareTag("NPC")) {
            GameController.SetCanvasVisibility(this.interaction, false);
            this.isInRange = false;
        }
    }

    private void ShowNextMessage() {
        this.currentIndex += this.messagesPerStep; 
        this.ShowMessage();

        if(this.currentIndex >= this.dynamicDialogue.Count) {
            this.CloseDialogue(); 
        }
    }

    private void ShowPreviousMessage() {
        this.currentIndex -= this.messagesPerStep; 
        if (this.currentIndex < 0)
            this.currentIndex = 0; 

        this.ShowMessage();
    }

    private void ShowMessage() {

        this.nameText.text = this.npcController.GetName();
        this.picture.sprite = this.npcController.GetPictureSprite();
        int endIndex = Mathf.Min(this.currentIndex + this.messagesPerStep, this.dynamicDialogue.Count); 
        this.dialogueText.text = ""; 
        for (int i = this.currentIndex; i < endIndex; i++) {
            this.dialogueText.text += this.dynamicDialogue[i] + "\n"; 
        }
    }

    private void CloseDialogue() {
        this.ManageDialogue(false);
    }

    private void StartDialogue() {

        this.currentIndex = 0; 
        this.ManageDialogue(true);
        this.SetCurrentDialogueSet(); 

        if(questController.currentQuest.GetType() == "Speaking" && currentQuestIndex == 0){

            dialogueInitiated = true;
            SetCurrentDialogueSet();
            
            questController.AddQuestFromDialogue(questList[currentQuestIndex].GetTitle(), questList[currentQuestIndex].GetDescription(), questList[currentQuestIndex].GetRequired(), questList[currentQuestIndex].GetType());
        }


        //TODO REVOIR
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
        if(this.currentDialogueSetIndex >= 0 && this.currentDialogueSetIndex < this.dialogueSets.Length) {
            this.dynamicDialogue = this.dialogueSets[this.currentDialogueSetIndex];
        }
    }

    private void ManageDialogue(bool state) {
        this.dialogueCanvas.enabled = state;
        GameController.SetGameState(!state);

        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        for(int i = 0; i < disabledCanvas.Length; i++)
            this.disabledCanvas[i].enabled = !state;
    }

    public bool GetDialogueInitiated() {
        return this.dialogueInitiated;
    }

    public void SetDialogueInitiated(bool state) {
        this.dialogueInitiated = state;
    }

    public void SetIsInRange(bool state) {
        GameController.SetCanvasVisibility(this.interaction, state);
        this.isInRange = state;
    }

    public void ResetCurrentQuestIndex() {
        this.currentQuestIndex = 0;
    }
}