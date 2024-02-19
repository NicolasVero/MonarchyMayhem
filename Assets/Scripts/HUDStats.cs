using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDStats : MonoBehaviour {

    [Header("Text Mesh")]
    [SerializeField] private TextMeshProUGUI levelStat;
    [SerializeField] private TextMeshProUGUI healthStat;
    [SerializeField] private TextMeshProUGUI killStat;
    [SerializeField] private TextMeshProUGUI attackStat;
    [SerializeField] private TextMeshProUGUI attackSpeedStat;
    [SerializeField] private TextMeshProUGUI rangeStat;
    [SerializeField] private TextMeshProUGUI regenerationStat;
    [SerializeField] private TextMeshProUGUI resistanceStat;
    [SerializeField] private TextMeshProUGUI speedStat;
    [SerializeField] private TextMeshProUGUI knockbackStat;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI difficultyName;

    [Header("Secondary Text Mesh")]
    [SerializeField] private TextMeshProUGUI weaponAttack;
    [SerializeField] private TextMeshProUGUI weaponAttackSpeed;
    [SerializeField] private TextMeshProUGUI weaponRange;
    [SerializeField] private TextMeshProUGUI weaponRegeneration;
    [SerializeField] private TextMeshProUGUI weaponSpeed;
    [SerializeField] private TextMeshProUGUI weaponKnockback;


    [Header("Images")]
    [SerializeField] private RawImage attackIcon;
    [SerializeField] private RawImage attackSpeedIcon;
    [SerializeField] private RawImage rangeIcon;
    [SerializeField] private RawImage regenerationIcon;
    [SerializeField] private RawImage resistanceIcon;
    [SerializeField] private RawImage speedIcon;
    [SerializeField] private RawImage knockbackIcon;
    [SerializeField] private RawImage enableAttackIcon;
    [SerializeField] private RawImage difficultyIcon;

    private Difficulty difficultyController;

    [Header("Joueur")]
    [SerializeField] private PlayerController player;

    private readonly string iconsPath = "Interface/Icons/"; 


    void Start() {
        this.UpdateStats();
        this.SetDifficulty();
    }

    private void SetDifficulty() {
        this.difficultyController = FindObjectOfType<Difficulty>();
        this.difficultyController.DisableChoice();

        this.difficultyIcon.texture = Resources.Load<Texture2D>(this.iconsPath + this.difficultyController.GetDifficulty());
        string name = "";

        if(this.difficultyController.GetDifficulty() == "easy") name = "Agitation"; 
        if(this.difficultyController.GetDifficulty() == "medium") name = "Soulèvement"; 
        if(this.difficultyController.GetDifficulty() == "hard") name = "Insurrection";

        this.difficultyName.text = name; 
    }

    public void UpdateHealth() {
        this.healthStat.text = (this.player.GetHealth() >= 0) ? this.player.GetHealth() + " / " + this.player.GetMaxActualHealth() : "0 / " + this.player.GetMaxActualHealth();
    }

    public void UpdateStats() {
        this.UpdateHealth();
        this.levelStat.text        = "" + this.player.GetLevel();
        this.killStat.text         = "" + this.player.GetKillCounter();
        this.attackStat.text       = "" + this.player.GetAttack();
        this.attackSpeedStat.text  = "" + this.player.GetAttackSpeed();
        this.rangeStat.text        = "" + this.player.GetRange();
        this.resistanceStat.text   = "" + this.player.GetResistance();
        this.speedStat.text        = "" + this.player.GetSpeed();
        this.knockbackStat.text    = "" + this.player.GetKnockback();
        this.regenerationStat.text = "" + this.player.GetRegeneration();
        this.weaponName.text       = "" + this.player.GetWeaponName();

        this.weaponAttack.text = "";
        this.weaponRange.text = "";
        this.weaponAttackSpeed.text = "";
        this.weaponSpeed.text = "";
        this.weaponRegeneration.text = "";
        this.weaponKnockback.text = "";

        if(this.player.GetWeaponAttack() != 0) {
            this.weaponAttack.text = "" + this.player.GetWeaponAttack();
            this.weaponAttack.color = (this.player.GetWeaponAttack() > 0) ? Color.green : Color.red;
        }

        if(this.player.GetWeaponRange() != 0) {
            this.weaponRange.text = "" + this.player.GetWeaponRange();
            this.weaponRange.color = (this.player.GetWeaponRange() > 0) ? Color.green : Color.red;
        }
    
        if(this.player.GetWeaponAttackSpeed() != 0) {
            this.weaponAttackSpeed.text = "" + this.player.GetWeaponAttackSpeed();
            this.weaponAttackSpeed.color = (this.player.GetWeaponAttackSpeed() < 0) ? Color.green : Color.red;
        }

        if(this.player.GetWeaponSpeed() != 0) {
            this.weaponSpeed.text = "" + this.player.GetWeaponSpeed();
            this.weaponSpeed.color = (this.player.GetWeaponSpeed() > 0) ? Color.green : Color.red;
        }

        if(this.player.GetWeaponRegeneration() != 0) {
            this.weaponRegeneration.text = "" + this.player.GetWeaponRegeneration();
            this.weaponRegeneration.color = (this.player.GetWeaponRegeneration() > 0) ? Color.green : Color.red;
        }

        if(this.player.GetWeaponKnockback() != 0) {
            this.weaponKnockback.text = "" + this.player.GetWeaponKnockback();
            this.weaponKnockback.color = (this.player.GetWeaponKnockback() > 0) ? Color.green : Color.red;
        }
    }
    
    public void MaxAttack() {
        Texture2D attackTexture = Resources.Load<Texture2D>(this.iconsPath + "max_attack");
        if(attackTexture != null) {
            this.attackIcon.texture = attackTexture;
        }
    }

    public void MaxAttackSpeed() {
        Texture2D attackSpeedTexture = Resources.Load<Texture2D>(this.iconsPath + "max_attack_speed");
        if(attackSpeedTexture != null) {
            this.attackSpeedIcon.texture = attackSpeedTexture;
        }
    }

    public void MaxRange() {
        Texture2D rangeTexture = Resources.Load<Texture2D>(this.iconsPath + "max_range");
        if(rangeTexture != null) {
            this.rangeIcon.texture = rangeTexture;
        }
    }

    public void MaxResistance() {
        Texture2D resistanceTexture = Resources.Load<Texture2D>(this.iconsPath + "max_resistance");
        if(resistanceTexture != null) {
            this.resistanceIcon.texture = resistanceTexture;
        }
    }

    public void MaxSpeed() {
        Texture2D speedTexture = Resources.Load<Texture2D>(this.iconsPath + "max_speed");
        if(speedTexture != null) {
            this.speedIcon.texture = speedTexture;
        }
    }

    public void MaxRegeneration() {
        Texture2D regenerationTexture = Resources.Load<Texture2D>(this.iconsPath + "max_regeneration");
        if(regenerationTexture != null) {
            this.regenerationIcon.texture = regenerationTexture;
        }
    }

    public void MaxKnockback() {
        Texture2D knockbackTexture = Resources.Load<Texture2D>(this.iconsPath + "max_knockback");
        if(knockbackTexture != null) {
            this.knockbackIcon.texture = knockbackTexture;
        }
    }

    public void ChangeEnableAttackIcon(bool status) {
        string textureLink = status ? "Interface/Icons/can_attack" : "Interface/Icons/cant_attack";
        Texture2D enableAttackTexture = Resources.Load<Texture2D>(textureLink);

        this.enableAttackIcon.texture = enableAttackTexture;
    }
}