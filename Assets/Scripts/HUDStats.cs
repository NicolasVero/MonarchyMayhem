using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDStats : MonoBehaviour {

    [Header("Text Mesh")]
    [SerializeField] private TextMeshProUGUI levelStat;
    [SerializeField] private TextMeshProUGUI killStat;
    [SerializeField] private TextMeshProUGUI attackStat;
    [SerializeField] private TextMeshProUGUI attackSpeedStat;
    [SerializeField] private TextMeshProUGUI rangeStat;
    [SerializeField] private TextMeshProUGUI resistanceStat;
    [SerializeField] private TextMeshProUGUI speedStat;
    [SerializeField] private TextMeshProUGUI regenerationStat;

    [Header("Secondary Text Mesh")]
    [SerializeField] private TextMeshProUGUI weaponAttack;
    [SerializeField] private TextMeshProUGUI weaponAttackSpeed;
    [SerializeField] private TextMeshProUGUI weaponRange;
    [SerializeField] private TextMeshProUGUI weaponSpeed;


    [Header("Images")]
    [SerializeField] private RawImage attackIcon;
    [SerializeField] private RawImage attackSpeedIcon;
    [SerializeField] private RawImage rangeIcon;
    [SerializeField] private RawImage resistanceIcon;
    [SerializeField] private RawImage speedIcon;
    [SerializeField] private RawImage regenerationIcon;


    [Header("Auto attack")]
    [SerializeField] private RawImage autoAttackIcon;

    [Header("Joueur")]
    [SerializeField] private PlayerController player;

    private readonly string bannersPath = "Interface/Icons/"; 

    void Start() {
        this.UpdateStats();
    }

    public void UpdateStats() {
        levelStat.text        = "" + player.GetLevel();
        killStat.text         = "" + player.GetKillCounter();
        attackStat.text       = "" + player.GetAttack();
        attackSpeedStat.text  = "" + player.GetAttackSpeed();
        rangeStat.text        = "" + player.GetRange();
        resistanceStat.text   = "" + player.GetResistance();
        speedStat.text        = "" + player.GetSpeed();
        regenerationStat.text = "" + player.GetRegeneration();

        weaponAttack.text = "";
        weaponRange.text = "";
        weaponAttackSpeed.text = "";
        weaponSpeed.text = "";

        if(player.GetWeaponAttack() != 0) {
            weaponAttack.text = "" + player.GetWeaponAttack();
            weaponAttack.color = (player.GetWeaponAttack() > 0) ? Color.green : Color.red;
        }

        if(player.GetWeaponRange() != 0) {
            weaponRange.text = "" + player.GetWeaponRange();
            weaponRange.color = (player.GetWeaponRange() > 0) ? Color.green : Color.red;
        }
    
        if(player.GetWeaponAttackSpeed() != 0) {
            weaponAttackSpeed.text = "" + player.GetWeaponAttackSpeed();
            weaponAttackSpeed.color = (player.GetWeaponAttackSpeed() > 0) ? Color.green : Color.red;
        }

        if(player.GetWeaponSpeed() != 0) {
            weaponSpeed.text = "" + player.GetWeaponSpeed();
            weaponSpeed.color = (player.GetWeaponSpeed() > 0) ? Color.green : Color.red;
        }
    }
    
    public void MaxAttack() {
        Texture2D attackTexture = Resources.Load<Texture2D>(this.bannersPath + "max_attack");
        if(attackTexture != null) {
            this.attackIcon.texture = attackTexture;
        }
    }

    public void MaxAttackSpeed() {
        Texture2D attackSpeedTexture = Resources.Load<Texture2D>(this.bannersPath + "max_attack_speed");
        if(attackSpeedTexture != null) {
            this.attackSpeedIcon.texture = attackSpeedTexture;
        }
    }

    public void MaxRange() {
        Texture2D rangeTexture = Resources.Load<Texture2D>(this.bannersPath + "max_range");
        if(rangeTexture != null) {
            this.rangeIcon.texture = rangeTexture;
        }
    }

    public void MaxResistance() {
        Texture2D resistanceTexture = Resources.Load<Texture2D>(this.bannersPath + "max_resistance");
        if(resistanceTexture != null) {
            this.resistanceIcon.texture = resistanceTexture;
        }
    }

    public void MaxSpeed() {
        Texture2D speedTexture = Resources.Load<Texture2D>(this.bannersPath + "max_speed");
        if(speedTexture != null) {
            this.speedIcon.texture = speedTexture;
        }
    }

    public void MaxRegeneration() {
        Texture2D regenerationTexture = Resources.Load<Texture2D>(this.bannersPath + "max_regeneration");
        if(regenerationTexture != null) {
            this.regenerationIcon.texture = regenerationTexture;
        }
    }

    public void ChangeAutoAttackStatus(bool status) {
        string textureLink = status ? "Interface/icons/canAttack" : "Interface/icons/cantAttack";
        Texture2D attackAutoTexture = Resources.Load<Texture2D>(textureLink);
        if(attackAutoTexture != null) {
            this.autoAttackIcon.texture = attackAutoTexture;
        }
    }
}