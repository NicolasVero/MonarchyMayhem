using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDStats : MonoBehaviour {
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI  levelStat;
    [SerializeField] private TextMeshProUGUI  attackStat;
    [SerializeField] private TextMeshProUGUI  resistanceStat;
    [SerializeField] private TextMeshProUGUI  attackSpeedStat;
    [SerializeField] private TextMeshProUGUI  rangeStat;
    [SerializeField] private TextMeshProUGUI  speedStat;

    void Start() {
        this.updateStats();
    }

    public void updateStats() {
        levelStat.text       = player.getLevel()       + "";
        attackStat.text      = player.getAttack()      + "";
        resistanceStat.text  = player.getResistance()  + "";
        attackSpeedStat.text = player.getAttackSpeed() + "";
        rangeStat.text       = player.getRange()       + "";
        speedStat.text       = player.getSpeed()       + "";
    }
}