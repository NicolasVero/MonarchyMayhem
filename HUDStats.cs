using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDStats : MonoBehaviour
{
    public TextMeshProUGUI attackStat;
    public TextMeshProUGUI resistanceStat;
    public PlayerController player;

    void Start() {
        Debug.Log(player);
        attackStat.text = "Hard hetero";
        // resistanceStat = 
    }

    void Update() {
        
    }
}   