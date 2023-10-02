using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    int maxHealth = 50;
    int enemy_health;
    FloatingHB healthBar;
    [SerializeField] Slider slider_enemy;

    private void Awake()
    {
        player = GameObject.Find("Knight").transform;
        agent = GetComponent<NavMeshAgent>();
        
        healthBar = slider_enemy.GetComponent<FloatingHB>();
        // slider_enemy = GameObject.Find("Enemy_Healthbar").GetComponent<Slider>();
        
        enemy_health = maxHealth;
        healthBar.UpdateHealthBar(slider_enemy, enemy_health, maxHealth);
    }
    

    // Taking damage when attacked by player
    public void AttackEnemy(int dmgAmount){
        if (enemy_health > dmgAmount){
            enemy_health -= dmgAmount;
            healthBar.UpdateHealthBar(slider_enemy, enemy_health, maxHealth);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (player){
            agent.SetDestination(player.position);
        }
    }
}