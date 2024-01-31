[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    private bool completed = false;
    private int killsRequired;
    private int currentKills;
    private PlayerController playerController;

    public Quest(string title, string description, int killsRequired, PlayerController playerController)
    {
        this.title = title;
        this.description = description;
        this.killsRequired = killsRequired;
        this.currentKills = 0;
        this.playerController = playerController;
    }

   public string GetQuestDetails()
    {
        // Utilise la méthode GetKillCounter du PlayerController pour obtenir la valeur actuelle
        int currentKills = playerController.GetKillCounter();

        return "Quête : " + title + "\nDescription : " + description + "\nComplétée : " + (completed ? "Oui" : "Non") +
            "\nKills actuels : " + currentKills + "/" + killsRequired;
    }

    public void SetCompleted(bool value)
    {
        completed = value;
    }

       public bool IsComplete()
    {  
        int currentKills = playerController.GetKillCounter();
        return !completed && currentKills >= killsRequired;
    }

}
