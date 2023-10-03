using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject levelUpPanel;

    const float speed = 10f;
    const float sensitivity = 10;
	public bool canAttack = false;
    public int enemyKillCounter;

    // attributs
    int xp = -1;
    int level = 0;
    int health;
    int resistance = 75;
    int attack = 5;
    float attackSpeed = 2f;

    // controlleur attributs
    int xpRequired = 0;
    int maxHealth = 50;
    int maxLevel = 20;
    int maxResistance = 75;
    int maxAttack = 50;
    int minAttackSpeed = 10;

    private Animator _animator;

    FloatingHB healthBar;
    Slider slider_player;

    Rigidbody rb;
    GameObject sword;
    EnemyAiController enemy;


    void Awake() {



        // Affiche le nom du personnage
        Debug.Log(Names.MainCharacter);

        // this.levelUpPanel.SetActive(false);
        this.healthBar = GameObject.Find("Player_Healthbar").GetComponent<FloatingHB>();
        this.slider_player = GameObject.Find("Player_Healthbar").GetComponent<Slider>();

        // Initialization of health
        Debug.Log("Start");
        this.health = this.maxHealth;
        this.healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);

        _animator = GetComponentInChildren<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        this.sword = GameObject.Find("Sword");

        this.rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) 
            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }


    void FixedUpdate(){

        // Marche là où le perso regarde
        transform.Translate(Vector3.forward * PlayerController.speed * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * PlayerController.speed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        
        // ISO Cam
        float y = Input.GetAxis("Mouse X") * PlayerController.sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);
    }

    // Taking damage when triggering object
    public void TakeDamage(int dmgAmount){
        if(this.health > dmgAmount){
            this.health -= dmgAmount;
            this.healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);
        } else {
            _animator.SetTrigger("Death");
            //Destroy(gameObject);
            Debug.Log("Vous êtes mort.");
            // Time.timeScale = 0;
        }
    }

    // Healing when triggering object
    public void HealHB(int healAmount) {
        this.health += healAmount;
        if(this.health >= this.maxHealth) 
            this.health = this.maxHealth;
        
        this.healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);
    }


    public void XPGain(int xpAmount) {
        this.xp += xpAmount;
        Debug.Log(xp + " / " + xpRequired);
        
        if(this.xp >= this.xpRequired && this.level < this.maxLevel) {
            this.level++;
            this.xpRequired += XPRequired();

            // this.levelUpPanel.SetActive(true);

            updateAttributs();
        }
    }

    private int XPRequired() {
        return (int)(5 * Math.Pow(1.5, this.level - 1));
    }

    public void OnAttack(InputValue value){
        // Debug.Log("Attack");
        _animator.SetTrigger("Attack");
    }



    // Modifications gain level
    private void updateAttributs() {
        Debug.Log("LEVEL UP !");
        this.updateResistance();
        this.updateHealth();
        this.updateAttack();
    }

    private void updateResistance() {
        this.resistance += 10;
        if(this.resistance > this.maxResistance) this.resistance = this.maxResistance;
    }

    private void updateHealth() {
        this.health += 10;
        if(this.health > this.maxHealth) this.health = this.maxHealth;
        healthBar.UpdateHealthBar(this.slider_player, this.health, this.maxHealth);
    }

    private void updateAttack() {
        this.attack += 2;
        if(this.attack > this.maxAttack) this.attack = this.maxAttack;
    }


	

    public int getResistance() { return this.resistance; }
}
