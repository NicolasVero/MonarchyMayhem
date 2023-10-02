using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    const float speed = 10f;
    const float sensitivity = 10;
    const float attackSpeed = 2f;
	public bool canAttack = false;

    // attributs
    int xp = -1;
    int level = 0;
    int health;
    int resistance = 75;
    int attack = 5;

    // controlleur attributs
    int xpRequired = 0;
    int maxHealth = 50;
    int maxLevel = 20;
    int maxResistance = 75;
    int maxAttack = 50;


    FloatingHB healthBar;
    Slider slider_player;

    Rigidbody rb;
    GameObject sword;
    EnemyAiController enemy;


    void Awake(){
        healthBar = GameObject.Find("Player_Healthbar").GetComponent<FloatingHB>();
        slider_player = GameObject.Find("Player_Healthbar").GetComponent<Slider>();

        // Initialization of health
        Debug.Log("Start");
        this.health = maxHealth;
        healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        sword = GameObject.Find("Sword");

        rb = GetComponent<Rigidbody>();
    }

    // Taking damage when triggering object
    public void TakeDamage(int dmgAmount){
        if(this.health > dmgAmount){
            this.health -= dmgAmount;
            healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
        } else {
            Destroy(gameObject);
            Debug.Log("Vous êtes mort.");
            Time.timeScale = 0;
        }
    }

    // Healing when triggering object
    public void HealHB(int healAmount) {
        this.health += healAmount;
        if(this.health >= maxHealth) 
            this.health = maxHealth;
        
        healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
    }


    public void XPGain(int xpAmount) {
        xp += xpAmount;
        Debug.Log(xp + " / " + xpRequired);
        
        if(xp >= xpRequired && level < maxLevel) {
            level++;
            xpRequired += XPRequired();
            updateAttributs();
        }
    }

    private int XPRequired() {
        return (int)(5 * Math.Pow(1.5, level - 1));
    }

    
    void FixedUpdate(){
        // Marche là où le perso regarde
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * speed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        
        // ISO Cam
        float y = Input.GetAxis("Mouse X") * sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);


        // Pause mais sah j'comprends aps
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0){
            Time.timeScale = 1;
            Debug.Log("Pause désactivé.");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0){
            Time.timeScale = 0;
            Debug.Log("Jeu en pause.");
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log(sword.transform.eulerAngles.z);
            sword.transform.eulerAngles = new Vector3(sword.transform.eulerAngles.x, sword.transform.eulerAngles.y, 290);
			canAttack = true;
            StartCoroutine(AnimateSwordCharge());
        }

    }



    // Modifications gain level
    private void updateAttributs() {
        Debug.Log("LEVEL UP !");
        updateResistance();
        updateHealth();
        updateAttack();
    }

    private void updateResistance() {
        resistance += 10;
        if(resistance > maxResistance) resistance = maxResistance;
    }

    private void updateHealth() {
        this.health += 10;
        if(this.health > maxHealth) this.health = maxHealth;
        healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
    }

    private void updateAttack() {
        this.attack += 2;
        if(attack > maxAttack) attack = maxAttack;
    }




   	private IEnumerator AnimateSwordCharge(){
        var i = sword.transform.eulerAngles.z;
        while (i <= 310){
            sword.transform.eulerAngles = new Vector3(sword.transform.eulerAngles.x, sword.transform.eulerAngles.y, i);
            i++;
        	yield return new WaitForSeconds(.00001f);
        }
        StartCoroutine(AnimateSwordAttack());
    }
   	private IEnumerator AnimateSwordAttack(){
        var i = sword.transform.eulerAngles.z;
        while (i >= 250){
            sword.transform.eulerAngles = new Vector3(sword.transform.eulerAngles.x, sword.transform.eulerAngles.y, i);
            i--;
        	yield return new WaitForSeconds(.00005f);
        }
        StartCoroutine(AnimateSwordBack());
    }
  	private IEnumerator AnimateSwordBack(){
        var i = sword.transform.eulerAngles.z;
        while (i != 290){
            sword.transform.eulerAngles = new Vector3(sword.transform.eulerAngles.x, sword.transform.eulerAngles.y, i);
            i++;
        	yield return new WaitForSeconds(.0001f);
        }
		canAttack = false;
    }
	

    public int getResistance() { return resistance; }

}
