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
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questMessage;
    [SerializeField] private TextMeshProUGUI questProgression;
    [SerializeField] private CollectibleController collectibleController;


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
        quests.Add(new Quest("Elimination d'ennemis", "Eliminez 30 ennemis ", 30, "Killing",collectibleController));
        quests.Add(new Quest("Obtenir la clef", "Trouvez les 6 fragments de clef dans le niveau 1", 6, "Finding",collectibleController));
     
      
        
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
}