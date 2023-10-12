using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDStats : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI levelStat;
    public TextMeshProUGUI attackStat;
    public TextMeshProUGUI resistanceStat;
    public TextMeshProUGUI attackSpeedStat;

    void Start() {
        this.updateStats();
    }

    void Update() {
        
    }

    public void updateStats() {
        levelStat.text = player.getLevel() + "";
        attackStat.text = player.getAttack() + "";
        resistanceStat.text = player.getResistance() + "";
        attackSpeedStat.text = player.getAttackSpeed() + "";
    }
}