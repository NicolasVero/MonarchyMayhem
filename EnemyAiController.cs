using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    PlayerController pc;

    int maxHealth = 50;
    int enemy_health;
    FloatingHB healthBar;
    [SerializeField] Slider slider_enemy;

    private void Awake()
    {
        this.pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
        this.player = GameObject.Find(Names.MainCharacter).transform;
        this.agent = GetComponent<NavMeshAgent>();
        
        this.healthBar = this.slider_enemy.GetComponent<FloatingHB>();
        // slider_enemy = GameObject.Find("Enemy_Healthbar").GetComponent<Slider>();
        
        this.enemy_health = maxHealth;
        this.healthBar.UpdateHealthBar(slider_enemy, enemy_health, maxHealth);
    }
    

    // Taking damage when attacked by player
    public void AttackEnemy(int dmgAmount){
        if (this.enemy_health > dmgAmount){
            this.enemy_health -= dmgAmount;
            this.healthBar.UpdateHealthBar(this.slider_enemy, this.enemy_health, this.maxHealth);
        }
        else{
            Destroy(this.gameObject);
            this.pc.enemyKillCounter++;
        }
    }

    private void FixedUpdate()
    {
        if (this.player){
            this.agent.SetDestination(this.player.position);
        }
    }

    public void TakeDamage(int damage)
    {
        
        enemy_health -= damage;
        // Debug.Log(enemy_health);
        this.healthBar.UpdateHealthBar(slider_enemy, enemy_health, maxHealth);
        
        
        if (enemy_health <= 0)
        {

            Debug.Log("Ennemie mort");
        }
    }

    public int getHealth() { return this.enemy_health; }
}