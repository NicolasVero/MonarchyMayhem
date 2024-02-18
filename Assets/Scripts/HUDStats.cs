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
        difficultyController = FindObjectOfType<Difficulty>();
        difficultyController.DisableChoice();

        this.difficultyIcon.texture = Resources.Load<Texture2D>(this.iconsPath + this.difficultyController.GetDifficulty());
        string name = "";

        if(this.difficultyController.GetDifficulty() == "Easy") name = "Agitation"; 
        if(this.difficultyController.GetDifficulty() == "Medium") name = "Soulèvement"; 
        if(this.difficultyController.GetDifficulty() == "Hard") name = "Insurrection";

        this.difficultyName.text = name; 
    }

    public void UpdateHealth() {
        healthStat.text = player.GetHealth() + " / " + player.GetMaxActualHealth();
    }

    public void UpdateStats() {
        UpdateHealth();
        levelStat.text        = "" + player.GetLevel();
        killStat.text         = "" + player.GetKillCounter();
        attackStat.text       = "" + player.GetAttack();
        attackSpeedStat.text  = "" + player.GetAttackSpeed();
        rangeStat.text        = "" + player.GetRange();
        resistanceStat.text   = "" + player.GetResistance();
        speedStat.text        = "" + player.GetSpeed();
        knockbackStat.text    = "" + player.GetKnockback();
        regenerationStat.text = "" + player.GetRegeneration();
        weaponName.text       = "" + player.GetWeaponName();

        weaponAttack.text = "";
        weaponRange.text = "";
        weaponAttackSpeed.text = "";
        weaponSpeed.text = "";
        weaponRegeneration.text = "";
        weaponKnockback.text = "";

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
            weaponAttackSpeed.color = (player.GetWeaponAttackSpeed() < 0) ? Color.green : Color.red;
        }

        if(player.GetWeaponSpeed() != 0) {
            weaponSpeed.text = "" + player.GetWeaponSpeed();
            weaponSpeed.color = (player.GetWeaponSpeed() > 0) ? Color.green : Color.red;
        }

        if(player.GetWeaponRegeneration() != 0) {
            weaponRegeneration.text = "" + player.GetWeaponRegeneration();
            weaponRegeneration.color = (player.GetWeaponRegeneration() > 0) ? Color.green : Color.red;
        }

        if(player.GetWeaponKnockback() != 0) {
            weaponKnockback.text = "" + player.GetWeaponKnockback();
            weaponKnockback.color = (player.GetWeaponKnockback() > 0) ? Color.green : Color.red;
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