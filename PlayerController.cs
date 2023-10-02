using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    const float speed = 10f;

    const int attack = 5;
    const float attackSpeed = 2f;
	public bool canAttack = false;

    int health;
    int maxHealth = 50;

    int xp = -1;
    int niveau = 0;
    int xpRequired = 0;

    float vertical;
    float horizontal;
    float sensitivity = 10;

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
        if (this.health > dmgAmount){
            this.health -= dmgAmount;
            healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
        }
        else{
            Destroy(gameObject);
            Debug.Log("Vous êtes mort.");
            Time.timeScale = 0;
        }
    }

    // Healing when triggering object
    public void HealHB(int healAmount){
        if (this.health < (maxHealth-healAmount)){
            this.health += healAmount;
            healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
        }
        else if (this.health < maxHealth){
            this.health = maxHealth;
            healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
        }
        else Debug.Log("Votre vie est au maximum: " + this.health);
        
    }


    public void XPGain(int xpAmount) {
        xp += xpAmount;
        Debug.Log(xp + " / " + xpRequired);
        
        if(xp >= xpRequired) {
            niveau++;
            xpRequired += XPRequired();
            updateHealth();
        }

    }

    private int XPRequired() {
        return (int)(5 * Math.Pow(1.5, niveau - 1));
    }

    
    void FixedUpdate(){
// Marche dans la dir de la map
//         vertical = Input.GetAxisRaw("Vertical");
//         horizontal = Input.GetAxisRaw("Horizontal");
//         Vector3 mvmt = new Vector3(horizontal, 0, vertical).normalized;
//         rb.velocity = new Vector3(mvmt.x * speed, rb.velocity.y, mvmt.z * speed);


// Marche là où le perso regarde
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * speed * Time.fixedDeltaTime * Input.GetAxis("Horizontal"));
        
// Rotation flinguée
//         Vector3 desiredRotation = new Vector3(rb.velocity.x, 0, rb.velocity.z);
//         // Regarde dans la dernière direction enregistrée
//         if (desiredRotation != Vector3.zero){
//             Quaternion rotation = Quaternion.LookRotation(desiredRotation);
//             // Rotation Behaviour (1 = Harsh turn, 0.1 = Smoother)
//             transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
//         }
// Rotation flinguée n°2
        // transform.Rotate(Vector3.up * Time.fixedDeltaTime * speed*20 * (Input.GetAxis("Vertical") * Input.GetAxis("Horizontal")));

        // ISO Cam
        float y = Input.GetAxis("Mouse X") * sensitivity;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);
// FPS Cam
//         float x = -(Input.GetAxis("Mouse Y") * sensitivity);
//         float y = Input.GetAxis("Mouse X") * sensitivity;
//         transform.eulerAngles = new Vector3(transform.eulerAngles.x + x, transform.eulerAngles.y + y, 0);


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
            // enemy = GameObject.Find("Zombie").GetComponent<EnemyAiController>();
            // enemy.AttackEnemy(attack);
            Debug.Log(sword.transform.eulerAngles.z);
            sword.transform.eulerAngles = new Vector3(sword.transform.eulerAngles.x, sword.transform.eulerAngles.y, 290);
			canAttack = true;
            StartCoroutine(AnimateSwordCharge());

        }

    }



    // Modifications gain niveau

    private void updateSpeed() {
        
    }

    private void updateHealth() {
        this.health += 10;
        Debug.Log("LEVEL UP ! : " + this.health);
        healthBar.UpdateHealthBar(slider_player, this.health, maxHealth);
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
	

}
