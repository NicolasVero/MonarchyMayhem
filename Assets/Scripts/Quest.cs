[System.Serializable]
public class Quest {
    
    private string title;
    private string description;
    private bool completed = false;
    private int killsRequired;
    private int currentKills;
    private PlayerController playerController;


    public Quest(string title, string description, int killsRequired, PlayerController playerController) {
        this.title = title;
        this.description = description;
        this.killsRequired = killsRequired;
        this.currentKills = 0;
        this.playerController = playerController;
    }

    public string GetQuestDetails() {
        this.currentKills = playerController.GetKillCounter();

        return "Quête : " + this.title + "\nDescription : " + this.description + "\nComplétée : " + (this.completed ? "Oui" : "Non") +
            "\nKills actuels : " + this.currentKills + "/" + this.killsRequired;
    }

    public void SetCompleted(bool value) {
        this.completed = value;
    }

    public bool IsComplete() {
        return !this.completed && this.currentKills >= this.killsRequired;
    }

}
