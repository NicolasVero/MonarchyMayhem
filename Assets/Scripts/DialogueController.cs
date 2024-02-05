using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private bool isInRange = false;
    [Header("Canvas Settings")]
    [SerializeField] public Canvas dialogueCanvas;
    [SerializeField] public TextMeshProUGUI dialogueText;
    [SerializeField] public Button closeButton;
    [SerializeField] public Button nextButton;
    [SerializeField] public Button prevButton; // Nouveau bouton pour afficher le message précédent

    [Header("Canvas à Désactiver")]
    [SerializeField] public Canvas enableCanvas;
    [SerializeField] public Canvas enableCanvas2;


    private List<string> dialogueMessages = new List<string>(); // Liste des messages
    private int currentIndex = 0; // Indice du message en cours


    void Start()
    {
        CloseDialogue();

        dialogueMessages.Add("Message 0 pour set up la liste");// Il faut toujours un message pour incrémenter le nombre à 1 directement afin de faire une liste dynamique

        dialogueMessages.Add("Bonjour, aventurier !");
        dialogueMessages.Add("Bienvenue dans notre village !");
        dialogueMessages.Add("N'hésitez pas à explorer.");

         if (nextButton != null)
        {
            nextButton.onClick.AddListener(ShowNextMessage);
        }

        if (prevButton != null)
        {
            prevButton.onClick.AddListener(ShowPreviousMessage);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseDialogue);
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Appeler la fonction de dialogue ici
            StartDialogue();
        }
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

    private void StartDialogue()
    {
        currentIndex = 0; // Réinitialise l'indice au début du dialogue
        ShowNextMessage(); // Affiche le premier message
        dialogueCanvas.enabled = true;
        GameController.SetGameState(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        enableCanvas.enabled = false;
        enableCanvas2.enabled = false;
    }

     private void UpdateDialogueText(string newDialogue)
    {
        if (dialogueText != null)
        {
            dialogueText.text = newDialogue;
        }
    }

    private void ShowNextMessage()
    {
        if (currentIndex < dialogueMessages.Count -1)
        {
            currentIndex++;
            UpdateDialogueText(dialogueMessages[currentIndex]);
            Debug.Log("ShowNextMessage done" + currentIndex);
        }
        else
        {
            CloseDialogue();
        }
    }

     private void ShowPreviousMessage()
    {
        if (currentIndex > 1)
        {
            currentIndex--;
            UpdateDialogueText(dialogueMessages[currentIndex]);
            Debug.Log("ShowPM done"+ currentIndex);
        }
    }

    private void CloseDialogue()
    {
        dialogueCanvas.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameController.SetGameState(true);
        enableCanvas.enabled = true;
        enableCanvas2.enabled = false;
    }
}
