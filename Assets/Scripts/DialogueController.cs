using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour {
    
    [SerializeField] private bool isInRange = false;

    [Header("Canvas Settings")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Canvas interaction;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Canvas to Deactivate")]
    [SerializeField] private Canvas[] disabledCanvas;

    private List<string> dialogueMessages = new List<string>();
    private int currentIndex;


    void Start() {
        GameController.SetCanvasVisibility(dialogueCanvas, false);

        dialogueMessages.Add("Bonjour, aventurier !");
        dialogueMessages.Add("Bienvenue dans notre village !");
        dialogueMessages.Add("N'hésitez pas à explorer.");

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

    private void OnTriggerEnter(Collider other) {
        isInRange = other.CompareTag("NPC");
    }

    private void OnTriggerExit(Collider other) {
        isInRange = !other.CompareTag("NPC");
    }


    private void ShowNextMessage() {
        this.currentIndex++;
        ShowMessage();
    }

    private void ShowPreviousMessage() {
        if (this.currentIndex > 0) {
            this.currentIndex--;
            ShowMessage();
        }
    }

    private void ShowMessage() {

        if(!(this.currentIndex < dialogueMessages.Count)) { 
            CloseDialogue();
            return;
        }

        dialogueText.text = dialogueMessages[this.currentIndex];
    }

    private void CloseDialogue() {
        ManageDialogue(false);
    }

    private void StartDialogue() {
        this.currentIndex = -1; 
        ShowNextMessage(); 
        ManageDialogue(true);
    }

    private void ManageDialogue(bool state) {
        dialogueCanvas.enabled = state;
        GameController.SetGameState(!state);

        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        for(int i = 0; i < disabledCanvas.Length; i++)
            disabledCanvas[i].enabled = !state;
    }
}