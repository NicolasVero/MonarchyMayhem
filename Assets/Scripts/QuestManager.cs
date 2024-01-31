using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText;
    private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;
    private Quest currentQuest;

    [Header("Joueur")]
    [SerializeField] private PlayerController player;
    private int KillCounter;

    void Start()
    {
        // Initialiser les quêtes ici
        InitializeQuests();
        
        // Afficher la première quête au démarrage
        ShowCurrentQuest();
    }

    void Update()
    {
        ShowCurrentQuest();
        CompleteCurrentQuest();
        
    }


    void InitializeQuests()
    {
        // Pas besoin de récupérer KillCounter ici, il sera obtenu dynamiquement dans GetQuestDetails
        quests.Add(new Quest("Élimination d'ennemis", "Éliminez 2 ennemis dans la forêt",2, player));
        
    }

    void ShowCurrentQuest()
    {
        // Afficher la quête actuelle dans le canvas
        currentQuest = quests[currentQuestIndex];
        UpdateQuestText();

    }

       public void CompleteCurrentQuest()
    {
        // Vérifie si la quête est complète
        if (currentQuest.IsComplete())
        {
            // Marque la quête comme complétée
            currentQuest.SetCompleted(true);

            // Passe à la quête suivante (si disponible)
            currentQuestIndex++;
            if (currentQuestIndex < quests.Count)
            {
                ShowCurrentQuest();
            }
            else
            {
                // Toutes les quêtes ont été complétées
                Debug.Log("Toutes les quêtes ont été complétées. Passage au niveau suivant...");
            }
        }

        // Mise à jour du texte de la quête
        UpdateQuestText();
    }

    void UpdateQuestText()
    {
        questText.text = currentQuest.GetQuestDetails();
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest;
    }
}
