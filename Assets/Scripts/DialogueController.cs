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
        GameController.SetCanvasVisibility(this.dialogueCanvas, false);

        this.dialogueMessages.Add("Bonjour, aventurier !");
        this.dialogueMessages.Add("Bienvenue dans notre village !");
        this.dialogueMessages.Add("N'hésitez pas à explorer.");

        this.nextButton.onClick.AddListener(ShowNextMessage);
        this.prevButton.onClick.AddListener(ShowPreviousMessage);
        this.closeButton.onClick.AddListener(CloseDialogue);
    }

    void Update() {

        if(this.isInRange && Input.GetKeyDown(KeyCode.E))
            this.StartDialogue();

        this.Interaction();
    }

    private void Interaction() {
        this.interaction.enabled = this.isInRange && !this.dialogueCanvas.enabled;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("NPC")) {
            this.isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("NPC")) {
            this.isInRange = false;
        }
    }

    private void ShowNextMessage() {
        this.currentIndex++;
        this.ShowMessage();
    }

    private void ShowPreviousMessage() {
        if (this.currentIndex > 0) {
            this.currentIndex--;
            this.ShowMessage();
        }
    }

    private void ShowMessage() {

        if(!(this.currentIndex < this.dialogueMessages.Count)) { 
            this.CloseDialogue();
            return;
        }

        this.dialogueText.text = this.dialogueMessages[this.currentIndex];
    }

    private void CloseDialogue() {
        this.ManageDialogue(false);
    }

    private void StartDialogue() {
        this.currentIndex = -1; 
        this.ShowNextMessage(); 
        this.ManageDialogue(true);
    }

    private void ManageDialogue(bool state) {
        this.dialogueCanvas.enabled = state;
        GameController.SetGameState(!state);

        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        for(int i = 0; i < this.disabledCanvas.Length; i++)
            this.disabledCanvas[i].enabled = !state;
    }
}