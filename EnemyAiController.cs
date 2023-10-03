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
        this.pc = GameObject.Find("Knight").GetComponent<PlayerController>();
        this.player = GameObject.Find("Knight").transform;
        this.agent = GetComponent<NavMeshAgent>();
        
        this.healthBar = slider_enemy.GetComponent<FloatingHB>();
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
}