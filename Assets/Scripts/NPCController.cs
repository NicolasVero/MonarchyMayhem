using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {
    
    [SerializeField] private int id;
    private NpcData npcData;

    void Start() {
        LoadNPCData();
    }

    private void LoadNPCData() {
        string jsonFilePath = "Data/NpcDatas"; 
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);


        if (jsonFile != null) {

            NpcDatas npcDatas = JsonUtility.FromJson<NpcDatas>(jsonFile.text);
            npcData = Array.Find(npcDatas.npcs, e => e.id == this.id);
            // Debug.Log(npcData);

            foreach (NpcData npcData in npcDatas.npcs) {
                // Debug.Log("NPC ID: " + npcData.id);

                foreach (Dialogue dialogue in npcData.dialogues) {
                    foreach (string message in dialogue.messages) {
                        // Debug.Log("Dialogue message: " + message);
                    }
                }

                foreach (CustomQuest quest in npcData.quests) {
                    // Debug.Log("Quest title: " + quest.title);
                    // Debug.Log("Quest description: " + quest.description);
                    // Debug.Log("Quest required: " + quest.required);
                    // Debug.Log("Quest type: " + quest.type);
                }
            }
        }
    }
}
