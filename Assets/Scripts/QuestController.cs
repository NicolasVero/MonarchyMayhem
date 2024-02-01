using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    private Quest currentQuest;
    private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;
    private int KillCounter;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI questText;
 
    void Start() {
        InitializeQuests();        
        ShowCurrentQuest();
    }

    void Update() {
        // ShowCurrentQuest();
        CompleteCurrentQuest();
    }


    void InitializeQuests()
    {
        // Pas besoin de récupérer KillCounter ici, il sera obtenu dynamiquement dans GetQuestDetails
        quests.Add(new Quest("Élimination d'ennemis", "Éliminez 2 ennemis dans la forêt", 2, "Killing"));
        quests.Add(new Quest("Obtenir la clé", "Trouvez les trois fragments de clé", 3, "Finding"));
        
    }

    void ShowCurrentQuest()
    {
        // Afficher la quête actuelle dans le canvas
        currentQuest = quests[currentQuestIndex];
        UpdateQuestText();

    }

    public void CompleteCurrentQuest() {

        if(currentQuest.IsComplete()) {

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
