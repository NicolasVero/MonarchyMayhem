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
    [SerializeField]private TextMeshProUGUI nameText;
    [SerializeField]private Image picture;
    [SerializeField]private Button closeButton;
    [SerializeField]private Button nextButton;
    [SerializeField]private Button prevButton;

    [Header("Canvas to Deactivate")]
    [SerializeField]private Canvas[] disabledCanvas;

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
            
            // this.picture.GetComponent<Image>().sprite = npcController.GetPictureSprite();
        }


        List<Canvas> combinedCanvasList = new List<Canvas>();
        Canvas questMenuCanvas = GameObject.Find("Quest Menu").GetComponent<Canvas>();
        Canvas hudCanvas = GameObject.Find("HUD").GetComponent<Canvas>();

        combinedCanvasList.Add(questMenuCanvas);
        combinedCanvasList.Add(hudCanvas);
        disabledCanvas = combinedCanvasList.ToArray();

        npcObject = GameObject.FindGameObjectWithTag("NPC");

        GameController.SetCanvasVisibility(dialogueCanvas, false);

        dialogueSets = npcController.GetDialogueSets();
        questList = npcController.GetQuestList();

        nextButton.onClick.AddListener(ShowNextMessage);
        prevButton.onClick.AddListener(ShowPreviousMessage);
        closeButton.onClick.AddListener(CloseDialogue);

        npcController = npcObject.GetComponent<NPCController>();
    }

    void Update() {

        if(isInRange && Input.GetKeyDown(KeyCode.E))
            StartDialogue();

        Interaction();
    }

    private void Interaction() {
        interaction.enabled = isInRange && !dialogueCanvas.enabled;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && npcObject != null && npcObject.CompareTag("NPC")) 
            isInRange = true;
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player") && npcObject != null && npcObject.CompareTag("NPC")) 
            isInRange = false;
    }

    private void ShowNextMessage() {
        this.currentIndex += messagesPerStep; 
        ShowMessage();

        if(this.currentIndex >= dynamicDialogue.Count) {
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

        nameText.text = npcController.GetName();
        picture.sprite = npcController.GetPictureSprite();
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

    public bool GetDialogueInitiated() {
        return dialogueInitiated;
    }

    public void SetDialogueInitiated(bool state) {
        this.dialogueInitiated = state;
    }

    public bool GetIsInRangeFalse() {
        return isInRange= false;
    }
}