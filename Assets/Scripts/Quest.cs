using UnityEngine;

public class Quest {
    
    private string type;
    private string title;
    private string description;
    private bool completed = false;
    private int required;
    private int currentCount;
    private PlayerController playerController;


    public Quest(string title, string description, int required, string type) {

        this.type = type;
        this.currentCount = 0;
        this.playerController = this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.title = title;
        this.description = description;
        this.required = required;
    }

    public string GetQuestDetails() {
        
        string message = "";

        if(this.type == "Killing") {
            this.currentCount = playerController.GetKillCounter();
            message = "Quête : " + this.title + "\nDescription : " + this.description + "\nComplétée : " + (this.completed ? "Oui" : "Non") +
            "\nKills actuels : " + this.currentCount + "/" + this.required;
        }

        if(this.type == "Finding") {
            this.currentCount = playerController.GetKillCounter();
            message = "Quête : " + this.title + "\nDescription : " + this.description + "\nComplétée : " + (this.completed ? "Oui" : "Non") +
            "\nMorceau trouvés : " + this.currentCount + "/" + this.required;
        }

        return message; 
    }

    public void SetCompleted(bool value) {
        this.completed = value;
    }

    public bool IsComplete() {
        return !this.completed && this.currentCount >= this.required;
    }

}
