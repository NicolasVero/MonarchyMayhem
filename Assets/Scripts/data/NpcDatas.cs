using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcDatas {
    public NpcData[] npc;
}


[Serializable]
public class NpcData {
    public List<string> messages;
    public string title;
    public string description;
    public int required;
    public string type;
}