using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private NpcData npcData; // Déclaration de la variable npcData au niveau de la classe

    void Start()
    {
        LoadNPCData();
    }

    void LoadNPCData()
    {
        string jsonFilePath = "Data/NpcDatas"; // Chemin relatif à partir du dossier Resources, sans extension
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        Debug.Log(jsonFile);

        if (jsonFile != null)
        {
            NpcDatas npcDatas = JsonUtility.FromJson<NpcDatas>(jsonFile.text);
            npcData = Array.Find(npcDatas.npcs, e => e.id == 1); // Utilisation de la variable de classe npcData
            Debug.Log(npcData);

            foreach (NpcData npcData in npcDatas.npcs) {
    Debug.Log("NPC ID: " + npcData.id);

    // Accéder aux dialogues
    foreach (Dialogue dialogue in npcData.dialogues) {
        foreach (string message in dialogue.messages) {
            Debug.Log("Dialogue message: " + message);
        }
    }

    // Accéder aux quêtes
    foreach (CustomQuest quest in npcData.quests) { // Notez que j'ai utilisé CustomQuest à la place de Quest
        Debug.Log("Quest title: " + quest.title);
        Debug.Log("Quest description: " + quest.description);
        Debug.Log("Quest required: " + quest.required);
        Debug.Log("Quest type: " + quest.type);
    }
}
        }
    }
}
