using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    private const float speed = 10f;
    private const float sensitivity = 10;
    private int enemyKillCounter;
	[SerializeField] private bool canAttack = false;

    // attributs
    private int totalXP = -1;
    private int xp = 0;
    private int xpToNext = 1;
    private int level = 0;
    private int health;
    private int resistance;
    private int attack = 5;
    private float attackSpeed = 2.0f;
    private float range = 3.0f;

    private float timeSinceLastAttack = 0f;
    // private float attackInterval = 2.0f;

    // controlleur attributs
    private int xpRequired = 0;
    private int maxHealth = 50;
    private int maxLevel = 20;
    private int maxResistance = 75;
    private int maxAttack = 50;
    private float maxRange = 4f;
    private float minAttackSpeed = 0.1f;

    public HealthBar healthBar;
    public XPBar xpBar;
    public HUDStats hudStats;

    public GameObject levelUpPanel;
    private GameController gameController;


    void Awake() {
        this.health = this.maxHealth;
        loadPlayerAttributes();
        GameController.setPanelVisibility(levelUpPanel, false);
    }

    void loadPlayerAttributes()
    {
        // Utilisez simplement le nom du fichier sans extension dans le dossier "Resources"
        string path = "PlayerBaseStats";
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        Debug.Log(jsonFile);

        if (jsonFile != null)
        {
            JsonUtility.FromJsonOverwrite(jsonFile.text, this);

            // Vous pouvez maintenant accéder directement aux propriétés
            Debug.Log("totalXP: " + totalXP);
            Debug.Log("xp: " + xp);
            Debug.Log("res: " + resistance);

            // ... (accédez aux autres propriétés de la même manière)
        }
        else
        {
            Debug.LogError("Fichier JSON non trouvé.");
        }
    }

    // void loadPlayerAttributes()
    // {
    //     string path = "Assets/Scripts/data/PlayerBaseStats.json";
    //     TextAsset jsonFile = Resources.Load<TextAsset>(path);

    //     Debug.Log(path);

    //     if (jsonFile != null)
    //     {
    //         JsonUtility.FromJsonOverwrite(jsonFile.text, this);

    //         // Vous pouvez maintenant accéder directement aux propriétés
    //         Debug.Log("totalXP: " + totalXP);
    //         Debug.Log("xp: " + xp);
    //         // ... (accédez aux autres propriétés de la même manière)
    //     } else {
    //         Debug.LogError("Fichier JSON non trouvé.");
    //     }
    // }

    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) 
            GameController.setGameState();

        // timerAttack();
    }

    void FixedUpdate() {

        transform.Translate(Vector3.forward * PlayerController.speed * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * PlayerController.speed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        
        float y = Input.GetAxis("Mouse X") * PlayerController.sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);

        timerAttack();
    }

    public void TakeDamage(int dmgAmount) {
        if(this.health > dmgAmount){
            this.health -= dmgAmount;
            this.healthBar.setHealthBar(this.health);
        } else {
            Debug.Log("Vous êtes mort.");
        }
    }

    public void HealHB(int healAmount) {
        this.health += healAmount;
        if(this.health >= this.maxHealth) 
            this.health = this.maxHealth;
        
        this.healthBar.setHealthBar(this.health);
    }

    public void XPGain(int xpAmount) {
        this.totalXP += xpAmount;
        this.xpBar.addXPBar(xpAmount);
        
        if(this.totalXP >= this.xpRequired && this.level < this.maxLevel) {
            this.level++;
            this.xpToNext = XPRequired();
            this.xpRequired += this.xpToNext;
            this.xp = 0;

            this.xpBar.setXPBarMax(this.xpToNext);
            this.xpBar.setXPBar(0);

            GameController.setGameState(false);
            GameController.setPanelVisibility(levelUpPanel, true);
            GameController.setCursorVisibility(true);
        }
    }

    private int XPRequired() {
        return (int)(5 * Math.Pow(1.5, this.level - 1));
    }

    public void updateResistance() {
        Debug.Log("Resistance améliorée");
        this.resistance += 10;
        if(this.resistance > this.maxResistance) this.resistance = this.maxResistance;
    }

    public void updateAttackSpeed() {
        Debug.Log("Vitesse attaque améliorée");
        this.attackSpeed -= 0.2f;
        if(this.attackSpeed < this.minAttackSpeed) this.attackSpeed = this.minAttackSpeed;
    }

    public void updateRange() {
        Debug.Log("range améliorée");
        this.range += 0.2f;
        if(this.range > this.maxRange) this.range = this.maxRange;
    }

     
    public void updateHealth() {
        Debug.Log("Vie améliorée");
        this.health += 10;
        if(this.health > this.maxHealth) 
            this.health = this.maxHealth;
            
        this.healthBar.setHealthBar(this.health);
    }

    public void updateAttack() {
        Debug.Log("Attaque améliorée");
        this.attack += 2;
        if(this.attack > this.maxAttack) this.attack = this.maxAttack;
    }

    void Attack() {
        //! OnColliderTrigger() 
        int temp = 0;

        Collider[]? hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in hitColliders) {
            EnemyAiController enemy = col.GetComponent<EnemyAiController>();

            if(enemy is EnemyAiController) {
                enemy.TakeDamage(attack);
            }
        }
    }

    public void timerAttack() {
        if (timeSinceLastAttack >= attackSpeed) {
            Attack();
            timeSinceLastAttack = 0f;
        }

        timeSinceLastAttack += Time.fixedDeltaTime;
    }

    public void incrementKillCounter() {
        this.enemyKillCounter++;
    }

    public int   getResistance()  { return this.resistance;  }
    public int   getAttack()      { return this.attack;      }
    public int   getHealth()      { return this.health;      }
    public int   getMaxHealth()   { return this.maxHealth;   }
    public int   getLevel()       { return this.level;       }
    public float getAttackSpeed() { return this.attackSpeed; }
    public float getRange()       { return this.range;       }
}
