using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    [SerializeField] private int id;
    private string name; 
    private List<string>[] dialogueSets;
    private List<Quest> questList;
    private string language;

    void Start() {

        if(this.id != 0)
            this.LoadNPCData();
    }

    private void LoadNPCData() {
        string jsonFilePath = (Application.systemLanguage == SystemLanguage.French) ? "Data/NpcDatasFR" : "Data/NpcDatasEN"; 
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        if (jsonFile != null) {
            NpcDatas npcDatas = JsonUtility.FromJson<NpcDatas>(jsonFile.text);
            NpcData npcData = Array.Find(npcDatas.npcs, e => e.id == this.id);

            this.name = npcData.name;
            
            this.dialogueSets = new List<string>[npcData.dialogues.Length];
            for(int i = 0; i < npcData.dialogues.Length; i++) {
                this.dialogueSets[i] = new List<string>(npcData.dialogues[i].messages);
            }

            this.questList = new List<Quest>();
            foreach(CustomQuest customQuest in npcData.quests) {
                this.questList.Add(new Quest(customQuest.title, customQuest.description, customQuest.required, customQuest.type));
            }
        }
    }

    public List<string>[] GetDialogueSets() {
        return this.dialogueSets;
    }

    public List<Quest> GetQuestList() {
        return this.questList;
    }

    public string GetName() {
        return this.name;
    }

    public Sprite GetPictureSprite() {
        return Resources.Load<Sprite>("Interface/NPC/npc_" + this.id);
    }
}
