using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private NpcData npcData;

    void Start()
    {
        LoadNPCData();
    }

    void LoadNPCData()
    {
        string jsonFilePath = "Data/NpcDatas"; // Chemin relatif à partir du dossier Resources, sans extension
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        Debug.Log(jsonFile);

        // if (jsonFile != null)
        // {
        NpcDatas npcDatas = JsonUtility.FromJson<NpcDatas>(jsonFile.text);
        // NpcData npcData = Array.Find(npcDatas.npcs, e => e.id == 1);
        // Debug.Log(npcData);

        //     // Recherche des données spécifiques pour ce NPC
        //     foreach (NpcData data in npcDatas.npcs)
        //     {
        //         if (data.id == GetComponent<NPC>().npcID) // Suppose que le NPCController est attaché au même GameObject que le composant NPC
        //         {
        //             npcData = data;
        //             return;
        //         }
        //     }

        //     Debug.LogWarning("NPC data not found for NPC ID: " + GetComponent<NPC>().npcID);
        // }
        // else
        // {
        //     Debug.LogError("NPC data file not found at path: " + jsonFilePath);
        // }
    }
}
