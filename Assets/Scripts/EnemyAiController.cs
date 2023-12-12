using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviour {
    
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] Slider slider_enemy;

    private PlayerController pc;

    private int maxHealth = 50;
    private int enemy_health;
    private FloatingHB healthBar;

    private void Awake() {
        this.pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
        this.player = GameObject.Find(Names.MainCharacter).transform;
        this.agent = GetComponent<NavMeshAgent>();
        
        this.healthBar = this.slider_enemy.GetComponent<FloatingHB>();
        
        this.enemy_health = maxHealth;
        this.healthBar.UpdateHealthBar(slider_enemy, enemy_health, maxHealth);
    }
    
    public void AttackEnemy(int dmgAmount){
        if (this.enemy_health > dmgAmount){
            this.enemy_health -= dmgAmount;
            this.healthBar.UpdateHealthBar(this.slider_enemy, this.enemy_health, this.maxHealth);
        } else{
            Destroy(this.gameObject);
            this.pc.incrementKillCounter();
        }
    }

    private void FixedUpdate() {
        if (this.player){
            this.agent.SetDestination(this.player.position);
        }
    }

    public void TakeDamage(int damage) {
        enemy_health -= damage;
        this.healthBar.UpdateHealthBar(slider_enemy, enemy_health, maxHealth);
                
        if (enemy_health <= 0)
            Debug.Log("Ennemie mort");
    }

    public void ApplyKnockback(float multiply) {
        Vector3 knockbackDirection = -transform.forward;
        float knockbackDistance = 1f * multiply; 
        float knockbackDuration = 0.2f; 

        StartCoroutine(KnockbackEffect(knockbackDirection, knockbackDistance, knockbackDuration));
    }

    IEnumerator KnockbackEffect(Vector3 direction, float distance, float duration) {
        float elapsed = 0f;

        while (elapsed < duration) {
            float step = distance * (Time.deltaTime / duration);
            transform.position += direction * step;

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    public int getHealth() { return this.enemy_health; }
}