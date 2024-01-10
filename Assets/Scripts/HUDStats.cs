using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDStats : MonoBehaviour {

    [Header("Text Mesh")]
    [SerializeField] private TextMeshProUGUI levelStat;
    [SerializeField] private TextMeshProUGUI attackStat;
    [SerializeField] private TextMeshProUGUI attackSpeedStat;
    [SerializeField] private TextMeshProUGUI rangeStat;
    [SerializeField] private TextMeshProUGUI resistanceStat;
    [SerializeField] private TextMeshProUGUI speedStat;

    [Header("Images")]
    [SerializeField] private RawImage attackIcon;
    [SerializeField] private RawImage attackSpeedIcon;
    [SerializeField] private RawImage rangeIcon;
    [SerializeField] private RawImage resistanceIcon;
    [SerializeField] private RawImage speedIcon;


    [Header("Joueur")]
    [SerializeField] private PlayerController player;


    void Start() {
        this.updateStats();
    }

    public void updateStats() {
        levelStat.text       = "" + player.getLevel();
        attackStat.text      = "" + player.getAttack();
        attackSpeedStat.text = "" + player.getAttackSpeed();
        rangeStat.text       = "" + player.getRange();
        resistanceStat.text  = "" + player.getResistance();
        speedStat.text       = "" + player.getSpeed();
    }
    
    public void maxAttack() {
        Texture2D attackTexture = Resources.Load<Texture2D>("Interface/icons/max_attack");
        if (attackTexture != null) {
            this.attackIcon.texture = attackTexture;
        }
    }

    public void maxAttackSpeed() {
        Texture2D attackSpeedTexture = Resources.Load<Texture2D>("Interface/icons/max_attack_speed");
        if (attackSpeedTexture != null) {
            this.attackSpeedIcon.texture = attackSpeedTexture;
        }
    }

    public void maxRange() {
        Texture2D rangeTexture = Resources.Load<Texture2D>("Interface/icons/max_range");
        if (rangeTexture != null) {
            this.rangeIcon.texture = rangeTexture;
        }
    }

    public void maxResistance() {
        Texture2D resistanceTexture = Resources.Load<Texture2D>("Interface/icons/max_resistance");
        if (resistanceTexture != null) {
            this.resistanceIcon.texture = resistanceTexture;
        }
    }

    public void maxSpeed() {
        Texture2D speedTexture = Resources.Load<Texture2D>("Interface/icons/max_speed");
        if (speedTexture != null) {
            this.speedIcon.texture = speedTexture;
        }
    }
}