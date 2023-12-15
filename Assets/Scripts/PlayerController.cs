using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour {
    
    private PlayerBaseStats playerBaseStats;
    private PlayerMaxStats playerMaxStats;
    private PlayerIncreaseStats playerIncreaseStats;
    [SerializeField] private LevelUpChoice levelUpChoice;

    private const float baseSpeed = 10f;
    private const float sensitivity = 10;
    private int enemyKillCounter;
	[SerializeField] private bool canAttack = false;
    private bool canResume = true;
    private bool isAlive = true;

    // attributs
    private int totalXP;
    private int xp;
    private int xpToNext;
    private int level;
    private int health;
    private int resistance;
    private int attack;
    private float attackSpeed;
    private float range;
    private float speed;
    private float knockback;

    private float timeSinceLastAttack = 0f;
    // private float attackInterval = 2.0f;

    // controlleur attributs
    private int xpRequired = 0;

    private int maxHealth;
    private int maxLevel;
    private int maxResistance;
    private int maxAttack;
    private float maxRange;
    private float minAttackSpeed;
    private float maxSpeed;
    private float maxKnockback;

    private int[] increaseHealth;
    private int[] increaseResistance;
    private int[] increaseAttack;
    private float[] increaseAttackSpeed;
    private float[] increaseRange;
    private float[] increaseSpeed;
    private float[] increaseKnockback;


    private int healthLevel = 1;
    private int resistanceLevel = 1;
    private int attackLevel = 1;
    private int attackSpeedLevel = 1;
    private int rangeLevel = 1;
    private int speedLevel = 1;

    public HealthBar healthBar;
    public XPBar xpBar;
    public HUDStats hudStats;

    public GameObject levelUpPanel;
    private GameController gameController;


    void Awake() {
        this.health = this.maxHealth;
        loadAttributes();
        GameController.setPanelVisibility(levelUpPanel, false);
    }

    public void loadAttributes() {
        TextAsset baseStats     = Resources.Load<TextAsset>("Data/PlayerBaseStats");
        TextAsset maxStats      = Resources.Load<TextAsset>("Data/PlayerMaxStats");
        TextAsset increaseStats = Resources.Load<TextAsset>("Data/PlayerIncreaseStats");
        
        if(baseStats != null && maxStats != null && increaseStats != null) {
            PlayerBaseStats playerBaseStats = JsonUtility.FromJson<PlayerBaseStats>(baseStats.text);
            PlayerMaxStats playerMaxStats = JsonUtility.FromJson<PlayerMaxStats>(maxStats.text);
            PlayerIncreaseStats playerIncreaseStats = JsonUtility.FromJson<PlayerIncreaseStats>(increaseStats.text);

            this.totalXP             = playerBaseStats.totalXP;
            this.xp                  = playerBaseStats.xp;
            this.xpToNext            = playerBaseStats.xpToNext;
            this.level               = playerBaseStats.level;
            this.health              = playerBaseStats.health;
            this.resistance          = playerBaseStats.resistance;
            this.attack              = playerBaseStats.attack;
            this.attackSpeed         = playerBaseStats.attackSpeed;
            this.range               = playerBaseStats.range;
            this.speed               = playerBaseStats.speed;
            this.knockback           = playerBaseStats.knockback;
     
            this.maxHealth           = playerMaxStats.maxHealth;
            this.maxLevel            = playerMaxStats.maxLevel;
            this.maxResistance       = playerMaxStats.maxResistance;
            this.maxAttack           = playerMaxStats.maxAttack;
            this.maxRange            = playerMaxStats.maxRange;
            this.minAttackSpeed      = playerMaxStats.minAttackSpeed;
            this.maxSpeed            = playerMaxStats.maxSpeed;
            this.maxKnockback        = playerMaxStats.maxKnockback;

            this.increaseHealth      = playerIncreaseStats.increaseHealth;
            this.increaseResistance  = playerIncreaseStats.increaseResistance;
            this.increaseAttack      = playerIncreaseStats.increaseAttack;
            this.increaseAttackSpeed = playerIncreaseStats.increaseAttackSpeed;
            this.increaseRange       = playerIncreaseStats.increaseRange;
            this.increaseSpeed       = playerIncreaseStats.increaseSpeed;
            this.increaseKnockback   = playerIncreaseStats.increaseKnockback;

            Debug.Log("playerIncreaseStats.increaseRange");
            Debug.Log(playerIncreaseStats.increaseRange[0]);

            // this.increaseHealth      = playerIncreaseStats.increaseHealth;
            // this.increaseResistance  = playerIncreaseStats.increaseResistance;
            // this.increaseAttack      = playerIncreaseStats.increaseAttack;
            // this.increaseAttackSpeed = playerIncreaseStats.increaseAttackSpeed;
            // this.increaseRange       = playerIncreaseStats.increaseRange;
            // this.increaseSpeed       = playerIncreaseStats.increaseSpeed;
            // this.increaseKnockback   = playerIncreaseStats.increaseKnockback;
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.P) && this.canResume) 
            GameController.setGameState();

        // timerAttack();
        DrawCircleAroundPlayer();
    }

    void FixedUpdate() {

        transform.Translate(this.speed * PlayerController.baseSpeed * Vector3.forward * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(this.speed * PlayerController.baseSpeed * Vector3.right   * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        
        float y = Input.GetAxis("Mouse X") * PlayerController.sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);

        timerAttack();
    }

    void Attack() {
        int temp = 0;

        Collider[]? hitColliders = Physics.OverlapSphere(transform.position, range);

        Debug.Log(transform.position);

        foreach (Collider col in hitColliders) {
            EnemyAiController enemy = col.GetComponent<EnemyAiController>();

            if(enemy is EnemyAiController) {
                enemy.TakeDamage(attack);
                enemy.ApplyKnockback(this.knockback);
            }
        }
    }

    public void timerAttack() {
        if(this.timeSinceLastAttack >= this.attackSpeed) {
            // Debug.Log("On the timer attack");   
            Attack();
            this.timeSinceLastAttack = 0f;
        }

        // Debug.Log("Timer attack");
        this.timeSinceLastAttack += Time.fixedDeltaTime;
    }

    void DrawCircleAroundPlayer() {
        const int numRays = 36;
        const float angleIncrement = 360.0f / numRays;

        for (int i = 0; i < numRays; i++) {
            float angle = i * angleIncrement;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * this.range;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * this.range;

            Vector3 rayDirection = new Vector3(x, 0.0f, z);
            Debug.DrawRay(transform.position, rayDirection, Color.red);
        }
    }

    public void TakeDamage(int dmgAmount) {
        if(this.health > dmgAmount){
            this.health -= dmgAmount;
            this.healthBar.setHealthBar(this.health);
        } else {
            Debug.Log("Vous êtes mort.");
            this.isAlive = false;
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

            this.setCanResume(false);

            this.levelUpChoice.updateStatsDisplay();
        }
    }

    public bool getCanResume() {
        return this.canResume && this.isAlive;
    }

    public void setCanResume(bool statut) {
        this.canResume = statut;
    }

    private int XPRequired() {
        // return (int)(5 * Math.Pow(1.5, this.level - 1));
        return 5;
    }

    public void updateResistance() {
        this.resistance += this.increaseResistance[this.resistanceLevel];
        this.resistanceLevel++;
        if(this.resistance > this.maxResistance) this.resistance = this.maxResistance;
    }

    public void updateAttackSpeed() {
        this.attackSpeed += this.increaseAttackSpeed[this.attackSpeedLevel - 1];
        this.attackSpeedLevel++;
        if(this.attackSpeed < this.minAttackSpeed) this.attackSpeed = this.minAttackSpeed;
    }

    public void updateRange() {
        this.range += this.increaseRange[this.rangeLevel - 1];
        this.rangeLevel++;
        if(this.range > this.maxRange) this.range = this.maxRange;
    }

    public void updateHealth() {
        this.health += this.increaseHealth[this.healthLevel - 1];
        this.healthLevel++;
        if(this.health > this.maxHealth) 
            this.health = this.maxHealth;
            
        this.healthBar.setHealthBar(this.health);
    }

    public void updateAttack() {
        this.attack += this.increaseAttack[this.attackLevel - 1];
        this.knockback += this.increaseKnockback[this.attackLevel - 1];
        this.attackLevel++;
        if(this.attack > this.maxAttack) this.attack = this.maxAttack;
        if(this.knockback > this.maxKnockback) this.knockback = this.maxKnockback;

    }

    public void updateSpeed() {
        this.speed += this.increaseSpeed[this.speedLevel];
        this.speedLevel++;
        if(this.speed > this.maxSpeed) this.speed = this.maxSpeed;
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
    public float getSpeed()       { return this.speed;       }

    public int getHealthLevel()      { return this.healthLevel;      }
    public int getResistanceLevel()  { return this.resistanceLevel;  }
    public int getAttackLevel()      { return this.attackLevel;      }
    public int getAttackSpeedLevel() { return this.attackSpeedLevel; }
    public int getRangeLevel()       { return this.rangeLevel;       }
    public int getSpeedLevel()       { return this.speedLevel;       }
}
