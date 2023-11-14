using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    const float speed = 10f;
    const float sensitivity = 10;
	public bool canAttack = false;
    public int enemyKillCounter;

    // attributs
    int total_xp = -1;
    int xp = 0;
    int xpToNext = 1;
    int level = 0;
    int health;
    int resistance = 0;
    int attack = 5;
    float attackSpeed = 2.0f;
    float range = 3.0f;

    private float timeSinceLastAttack = 0f;
    // private float attackInterval = 2.0f;

    // controlleur attributs
    int xpRequired = 0;
    int maxHealth = 50;
    int maxLevel = 20;
    int maxResistance = 75;
    int maxAttack = 50;
    // int minAttackSpeed = 10;
    float maxRange = 4f;
    float minAttackSpeed = 0.1f;

    private Animator _animator;

    public HealthBar healthBar;
    public XPBar xpBar;

    public HUDStats hudStats;

    Rigidbody rb;
    GameObject sword;
    EnemyAiController enemy;

    public GameObject levelUpPanel;
    GameController gameController;


    void Awake() {

        // this.levelUpPanel.SetActive(false);
        // this.healthBar = GameObject.Find("Player_Healthbar").GetComponent<FloatingHB>();
        // this.slider_player = GameObject.Find("Player_Healthbar").GetComponent<Slider>();

        // Initialization of health
        Debug.Log("Start");
        this.health = this.maxHealth;
        // this.healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);

        _animator = GetComponentInChildren<Animator>();

        this.sword = GameObject.Find("Sword");
        GameController.setPanelVisibility(levelUpPanel, false);

        this.rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) 
            GameController.setGameState();


        // timerAttack();
    }

    void FixedUpdate() {

        // Marche là où le perso regarde
        transform.Translate(Vector3.forward * PlayerController.speed * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * PlayerController.speed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        
        // ISO Cam
        float y = Input.GetAxis("Mouse X") * PlayerController.sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);

        timerAttack();

        // Attack();
        // InvokeRepeating("Attack", 2.0f, attackSpeed);
        // attackTimer += Time.fixedDeltaTime;

        // // Si le compteur de temps dépasse l'intervalle, effectuez une attaque.
        // if (attackTimer >= attackSpeed) {
        //     Attack();
        //     // Réinitialisez le compteur de temps.
        //     attackTimer = 0f;
        // }
    
            // Augmentez le compteur de temps depuis la dernière attaque à chaque mise à jour FixedUpdate.

        // Si le temps écoulé dépasse l'intervalle d'attaque, effectuez une attaque.

    }

    // Taking damage when triggering object
    public void TakeDamage(int dmgAmount) {
        if(this.health > dmgAmount){
            this.health -= dmgAmount;
            // this.healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);
            this.healthBar.setHealthBar(this.health);
        } else {
            // _animator.SetTrigger("Death");
            Debug.Log("Vous êtes mort.");
        }
    }

    // Healing when triggering object
    public void HealHB(int healAmount) {
        this.health += healAmount;
        if(this.health >= this.maxHealth) 
            this.health = this.maxHealth;
        
        // this.healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);
        this.healthBar.setHealthBar(this.health);
    }


    public void XPGain(int xpAmount) {
        this.total_xp += xpAmount;
        this.xpBar.addXPBar(xpAmount);
        // Debug.Log(total_xp + " / " + xpRequired);
        
        if(this.total_xp >= this.xpRequired && this.level < this.maxLevel) {
            this.level++;
            this.xpToNext = XPRequired();
            this.xpRequired += this.xpToNext;
            this.xp = 0;

            Debug.Log(this.xpBar);

            this.xpBar.setXPBarMax(this.xpToNext);
            this.xpBar.setXPBar(0);


            GameController.setGameState(false);
            GameController.setPanelVisibility(levelUpPanel, true);
            GameController.setCursorVisibility(true);
            // updateAttributs();
        }
    }

    private int XPRequired() {
        return (int)(5 * Math.Pow(1.5, this.level - 1));
    }

    // public void OnAttack(InputValue value){
    //     _animator.SetTrigger("Attack");
    // }



    // Modifications gain level
    // private void updateAttributs() {
        // this.updateResistance();
        // this.updateHealth();
        // this.updateAttack();
    // }

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
            
        // healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);
        this.healthBar.setHealthBar(this.health);
    }

    public void updateAttack() {
        Debug.Log("Attaque améliorée");
        this.attack += 2;
        if(this.attack > this.maxAttack) this.attack = this.maxAttack;
    }

    void Attack()
    {

        //! OnColliderTrigger() 
        int temp = 0;

        Collider[]? hitColliders = Physics.OverlapSphere(transform.position, range);

        Debug.Log("Nombre d'ennemis touchés : " + hitColliders.Length);
        // Debug.Log(hitColliders.Length);
        // Parcourez les ennemis trouvés
        // Debug.Log("-- NEW --");
        // Debug.Log("LEN : " + hitColliders.Length);

        foreach (Collider col in hitColliders) {
            EnemyAiController enemy = col.GetComponent<EnemyAiController>();
            Debug.Log(enemy);

            if(enemy is EnemyAiController) {
                // Debug.Log("on attack");
                temp++;
                enemy.TakeDamage(attack);
            }
        }

        Debug.Log("Nombre de zombies : " + temp);
    }

    public void timerAttack() {
        if (timeSinceLastAttack >= attackSpeed) {
            // Debug.Log("attaque");
            Attack();
            // Réinitialisez le temps écoulé.
            timeSinceLastAttack = 0f;
        }
        timeSinceLastAttack += Time.fixedDeltaTime;
    }

    public int   getResistance()  { return this.resistance;  }
    public int   getAttack()      { return this.attack;      }
    public int   getHealth()      { return this.health;      }
    public int   getMaxHealth()   { return this.maxHealth;   }
    public int   getLevel()       { return this.level;       }
    public float getAttackSpeed() { return this.attackSpeed; }
    public float getRange()       { return this.range;       }
}
