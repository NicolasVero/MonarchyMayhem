using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour {
    
    [Header("HUD")] 
    [SerializeField] private Slider xpBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject levelUpPanel;

    [Header("Scripts")]
    [SerializeField] private LevelUpChoice levelUpChoice;
    [SerializeField] private HUDStats hudStats;


    private PlayerBaseStats playerBaseStats;
    private PlayerMaxStats playerMaxStats;
    private PlayerIncreaseStats playerIncreaseStats;

    private const float sensitivity = 10;
    private int enemyKillCounter;
    private bool goingAttack = false;
	private bool canAttack = true;
    private bool canResume = true;
    private bool isAlive = true;
    private bool enableAutomaticAttack = true;


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

    private SphereCollider rangeCollider;
    private Animator animator;
    private Vector3 moveDirection;


    void Awake() {
        this.setHealthBarMax(this.maxHealth);
        this.rangeCollider = GetComponent<SphereCollider>();
        this.animator = GetComponentInChildren<Animator>();
        GameController.setPanelVisibility(this.levelUpPanel, false);
        loadAttributes();

        this.xpBar.maxValue = 1;
        this.xpBar.value = 0;

        this.healthBar.maxValue = this.getMaxHealth();
        this.healthBar.value = this.getHealth();
    }

    public void loadAttributes() {
        TextAsset baseStats     = Resources.Load<TextAsset>("Data/PlayerBaseStats");
        TextAsset maxStats      = Resources.Load<TextAsset>("Data/PlayerMaxStats");
        TextAsset increaseStats = Resources.Load<TextAsset>("Data/PlayerIncreaseStats");

        if(baseStats != null && maxStats != null && increaseStats != null) {
            PlayerBaseStats playerBaseStats = JsonUtility.FromJson<PlayerBaseStats>(baseStats.text);
            PlayerMaxStats playerMaxStats = JsonUtility.FromJson<PlayerMaxStats>(maxStats.text);
            PlayerIncreaseStats playerIncreaseStats = JsonUtility.FromJson<PlayerIncreaseStats>(increaseStats.text);


            this.totalXP              = playerBaseStats.totalXP;
            this.xp                   = playerBaseStats.xp;
            this.xpToNext             = playerBaseStats.xpToNext;
            this.level                = playerBaseStats.level;
            this.health               = playerBaseStats.health;
            this.resistance           = playerBaseStats.resistance;
            this.attack               = playerBaseStats.attack;
            this.attackSpeed          = playerBaseStats.attackSpeed;
            this.range                = playerBaseStats.range;
            this.rangeCollider.radius = playerBaseStats.range;
            this.speed                = playerBaseStats.speed;
            this.knockback            = playerBaseStats.knockback;
     
            this.maxHealth            = playerMaxStats.maxHealth;
            this.maxLevel             = playerMaxStats.maxLevel;
            this.maxResistance        = playerMaxStats.maxResistance;
            this.maxAttack            = playerMaxStats.maxAttack;
            this.maxRange             = playerMaxStats.maxRange;
            this.minAttackSpeed       = playerMaxStats.minAttackSpeed;
            this.maxSpeed             = playerMaxStats.maxSpeed;
            this.maxKnockback         = playerMaxStats.maxKnockback;

            this.increaseHealth       = playerIncreaseStats.increaseHealth;
            this.increaseResistance   = playerIncreaseStats.increaseResistance;
            this.increaseAttack       = playerIncreaseStats.increaseAttack;
            this.increaseAttackSpeed  = playerIncreaseStats.increaseAttackSpeed;
            this.increaseRange        = playerIncreaseStats.increaseRange;
            this.increaseSpeed        = playerIncreaseStats.increaseSpeed;
            this.increaseKnockback    = playerIncreaseStats.increaseKnockback;
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.P) && this.canResume) 
            GameController.setGameState();

        if(Input.GetMouseButtonDown(0) && this.canResume && !GameController.gameIsFreeze()) {
            this.enableAutomaticAttack = !this.enableAutomaticAttack;
            this.hudStats.changeAutoAttackStatus(this.enableAutomaticAttack);
        }
        
        DrawCircleAroundPlayer();
    }

    void FixedUpdate() {

        this.Move();

        float y = Input.GetAxis("Mouse X") * PlayerController.sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);

        this.goingAttack = false;
        this.timerAttack();
    }

    public void timerAttack() {
        if(this.timeSinceLastAttack >= this.attackSpeed) {  
            this.goingAttack = true;
            this.timeSinceLastAttack = 0f;
        }

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

    public void TakeDamage(int damage) {

        this.health -= Mathf.RoundToInt(damage * 1.0f - (float) this.resistance / 100.0f);
        this.setHealthBar(this.health);

        if(this.health <= 0) {
            this.isAlive = false;
            this.animator.SetInteger("Death", UnityEngine.Random.Range(1, 4));
        }
    }

    public void HealHB(int healAmount) {
        this.health += healAmount;
        if(this.health >= this.maxHealth) 
            this.health = this.maxHealth;
        
        this.setHealthBar(this.health);
    }

    public void XPGain(int xpAmount) {
        this.totalXP += xpAmount;
        this.addXPBar(xpAmount);
        
        if(this.totalXP >= this.xpRequired && this.level < this.maxLevel) {
            this.level++;
            this.xpToNext = XPRequired();
            this.xpRequired += this.xpToNext;
            this.xp = 0;

            this.setXPBarMax(this.xpToNext);
            this.setXPBar(0);

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
        return (int)(5 * Math.Pow(1.2, this.level - 1));
    }

    public void updateResistance() {
        this.resistance += this.increaseResistance[this.resistanceLevel - 1];
        this.resistanceLevel++;
        if(this.resistance > this.maxResistance) this.resistance = this.maxResistance;
        if(this.resistanceLevel > 5) this.hudStats.maxResistance();
    }

    public void updateAttackSpeed() {
        this.attackSpeed += this.increaseAttackSpeed[this.attackSpeedLevel - 1];
        this.attackSpeedLevel++;
        if(this.attackSpeed < this.minAttackSpeed) this.attackSpeed = this.minAttackSpeed;
        if(this.attackSpeed > 5) this.hudStats.maxAttackSpeed();
    }

    public void updateRange() {
        this.range += this.increaseRange[this.rangeLevel - 1];
        this.rangeLevel++;
        if(this.range > this.maxRange) this.range = this.maxRange;
        if(this.rangeLevel > 5) this.hudStats.maxRange();

        this.rangeCollider.radius = this.range;
    }

    public void updateHealth() {
        this.health += this.increaseHealth[this.healthLevel - 1];
        this.healthLevel++;
        if(this.health > this.maxHealth) this.health = this.maxHealth;
            
        this.setHealthBar(this.health);
    }

    public void updateAttack() {
        this.attack += this.increaseAttack[this.attackLevel - 1];
        this.knockback += this.increaseKnockback[this.attackLevel - 1];
        this.attackLevel++;
        if(this.attack > this.maxAttack) this.attack = this.maxAttack;
        if(this.knockback > this.maxKnockback) this.knockback = this.maxKnockback;
        if(this.attackLevel > 5) this.hudStats.maxAttack();

    }

    public void updateSpeed() {
        this.speed += this.increaseSpeed[this.speedLevel - 1];
        this.speedLevel++;
        if(this.speed > this.maxSpeed) this.speed = this.maxSpeed;
        if(this.speedLevel > 5) this.hudStats.maxSpeed();
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


    private void setXPBar(int xp) {
        this.xpBar.value = xp;
    }

    private void addXPBar(int xp) {
        this.xpBar.value += xp;
    } 

    private void setXPBarMax(int max) {
        this.xpBar.maxValue = max;
    }

    private void setHealthBar(int hp) {
        this.healthBar.value = hp;
    }

    private void setHealthBarMax(int max) {
        this.healthBar.maxValue = max;
    }



    private void Move() {
        transform.Translate(Vector3.forward * this.getSpeed() * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * this.getSpeed() * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        float moveZ = Input.GetAxis("Vertical");
        this.moveDirection = new Vector3(0, 0, moveZ);
        this.moveDirection = this.transform.TransformDirection(this.moveDirection);

        moveAnims();
    }


    private void resetAnims() {
        this.animator.SetBool("Idle", false);
        this.animator.SetInteger("Walk", 0);
        this.animator.SetInteger("Strafe", 0);
        this.animator.SetInteger("Strafe_Forward", 0);
        this.animator.SetInteger("Strafe_Backward", 0);
    }

    private void moveAnims() {
        resetAnims();
        
        if (Input.GetAxis("Vertical") > 0) {
            if(Input.GetAxis("Horizontal") > 0) 
                this.animator.SetInteger("Strafe_Forward", 1);
            else if(Input.GetAxis("Horizontal") < 0) 
                this.animator.SetInteger("Strafe_Forward", -1);
            else 
                this.animator.SetInteger("Walk", 1);
            
        } else if(Input.GetAxis("Vertical") < 0) {
            if(Input.GetAxis("Horizontal") > 0)
                this.animator.SetInteger("Strafe_Backward", 1);
            else if (Input.GetAxis("Horizontal") < 0)
                this.animator.SetInteger("Strafe_Backward", -1);
            else
                this.animator.SetInteger("Walk", -1);
        } else {
            if(Input.GetAxis("Horizontal") > 0)
                this.animator.SetInteger("Strafe", 1);
            else if (Input.GetAxis("Horizontal") < 0)
                this.animator.SetInteger("Strafe", -1);
            else
                this.animator.SetBool("Idle", true);
        }
    }


    private void OnTriggerStay(Collider other) {
        if(this.goingAttack && this.canAttack && this.isAlive && this.enableAutomaticAttack && other.CompareTag(Names.BaseEnemy)) {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.TakeDamage(this.attack);
            enemy.ApplyKnockback(this.knockback);
            this.animator.SetTrigger("Attack");
        }
    }
}
