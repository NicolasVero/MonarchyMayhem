using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    [SerializeField] private int id;
    private string name; 
    private List<string>[] dialogueSets;
    private List<Quest> questList;

    void Start() {
        if(this.id != 0)
            LoadNPCData();
    }

    private void LoadNPCData() {
        string jsonFilePath = "Data/NpcDatas"; 
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        if (jsonFile != null){
            NpcDatas npcDatas = JsonUtility.FromJson<NpcDatas>(jsonFile.text);
            NpcData npcData = Array.Find(npcDatas.npcs, e => e.id == this.id);

            this.name = npcData.name;
            Debug.Log("NAAAAAAAAMEEEEE : " + this.name); 
            
            dialogueSets = new List<string>[npcData.dialogues.Length];
            for (int i = 0; i < npcData.dialogues.Length; i++) {
                dialogueSets[i] = new List<string>(npcData.dialogues[i].messages);
            }

            questList = new List<Quest>();
            foreach (CustomQuest customQuest in npcData.quests) {
                questList.Add(new Quest(customQuest.title, customQuest.description, customQuest.required, customQuest.type));
            }
        }
    }

    // Getter for dialogue sets
    public List<string>[] GetDialogueSets() {
        return dialogueSets;
    }

    // Getter for quest list
    public List<Quest> GetQuestList() {
        return questList;
    }

    public string GetName() {
        return this.name;
    }
}
