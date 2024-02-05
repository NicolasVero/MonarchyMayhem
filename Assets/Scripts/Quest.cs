using UnityEngine;

public class Quest {
    
    private string type;
    private string title;
    private string description;
    private bool completed = false;
    private int required;
    private int currentCount;
    private PlayerController playerController;
    private CollectibleController collectibleController;

    public Quest(string title, string description, int required, string type, CollectibleController collectibleController) {

        this.type = type;
        this.currentCount = 0;
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        // this.collectibleController = totalCollectedObjects.Collect<CollectibleController>();
        this.collectibleController = collectibleController;
        this.title = title;
        this.description = description;
        this.required = required;
    }

    public struct QuestDetails {
        public string YellowTitle { get; set; }
        public string Message { get; set; }
        public string Progression { get; set; }
    }

    public Quest.QuestDetails GetQuestDetails() {
        QuestDetails questDetails = new QuestDetails();

        if (this.type == "Killing") {
            this.currentCount = playerController.GetKillCounter();
            questDetails.YellowTitle = this.title;
            questDetails.Message = this.description ;
            questDetails.Progression = "Completee : " + "\t \t \t \t " + this.currentCount + "/" + this.required;
        }

         if (this.type == "Finding") {
            this.currentCount = collectibleController.GetCollectibleCounter();
            questDetails.YellowTitle = this.title;
            questDetails.Message = this.description ;
            questDetails.Progression = "Completee : " + "\t \t \t \t " + this.currentCount + "/" + this.required;
        }

        return questDetails;
    }


    public void SetCompleted(bool value) {
        this.completed = value;
    }

    public bool IsComplete() {
        return !this.completed && this.currentCount >= this.required;
    }

}