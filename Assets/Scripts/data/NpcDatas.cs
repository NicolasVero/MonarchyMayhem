using System;
using System.Collections.Generic;

[Serializable]
public class NpcDatas {
    public NpcData[] npcs;
}

[Serializable]
public class NpcData {
    public int id;
    public Dialogue[] dialogues;
    public CustomQuest[] quests;
}

[Serializable]
public class Dialogue {
    public string[] messages;
}

[Serializable]
public class CustomQuest {
    public string title;
    public string description;
    public int required;
    public string type;
}