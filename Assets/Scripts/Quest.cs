using UnityEngine;

public class Quest {
    
    private string type;
    private string title;
    private string description;
    private bool completed = false;
    private int required;
    private int currentCount;
    private bool currentBool;
    private PlayerController playerController;
    private CollectibleController collectibleController;
    private DialogueController dialogueController;
    private int countReset;

    public Quest(string title, string description, int required, string type) {

        this.type = type;
        this.currentCount = 0;
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.dialogueController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<DialogueController>();
        this.collectibleController = GameObject.FindGameObjectWithTag("Collectibles").GetComponent<CollectibleController>();  //ajouter un tag au parent collectible
        this.title = title;
        this.description = description;
        this.required = required;

        if(this.type == "Killing") this.countReset = playerController.GetKillCounter();
        if(this.type == "Finding") this.countReset =  collectibleController.GetCollectibleCounter();
    }

    public struct QuestDetails {
        public string YellowTitle { get; set; }
        public string Message { get; set; }
        public string Progression { get; set; }
    }

    public Quest.QuestDetails GetQuestDetails() {
        QuestDetails questDetails = new QuestDetails();

        if (this.type == "Killing") {
            this.currentCount = playerController.GetKillCounter() - this.countReset;
            questDetails.YellowTitle = this.title;
            questDetails.Message = this.description ;
            questDetails.Progression = "Completee : " + "\t \t \t \t " + this.currentCount + "/" + this.required;
        }

        if (this.type == "Finding") {
            this.currentCount = collectibleController.GetCollectibleCounter() - this.countReset;
            questDetails.YellowTitle = this.title;
            questDetails.Message = this.description ;
            questDetails.Progression = "Completee : " + "\t \t \t \t " + this.currentCount + "/" + this.required;
        }

        if (this.type == "Speaking") {
            this.currentBool = dialogueController.GetDialogueInitiated();
            questDetails.YellowTitle = this.title;
            questDetails.Message = this.description ;
            questDetails.Progression = "Completee : non" ;
        }
        return questDetails;
    }


    public void SetCompleted(bool value) {
        this.completed = value;
    }

    public bool IsComplete() {
        return !this.completed && this.currentCount >= this.required || !this.completed && this.currentBool == true;
    }

    public string GetType(){
        return this.type;
    }
    public string GetTitle(){
        return this.title;
    }
    public string GetDescription(){
        return this.description;
    }
    public int GetRequired(){
        return this.required;
    }    
}