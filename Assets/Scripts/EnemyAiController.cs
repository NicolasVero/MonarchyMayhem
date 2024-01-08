using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviour {
    
    private Transform player;
    private PlayerController playerController;
    private NavMeshAgent agent;

    private int health = 50;

    private void Awake() {
        this.agent = GetComponent<NavMeshAgent>();
        this.player = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
    }

    private void FixedUpdate() {
        if (this.player){
            this.agent.SetDestination(this.player.position);
        }
    }
    
    public void TakeDamage(int damage){

        this.health -= damage;

        if(this.health <= 0) {
            Destroy(this.gameObject);
            this.playerController.incrementKillCounter();
        }
    }

    public void ApplyKnockback(float multiply) {
        Vector3 knockbackDirection = -transform.forward;
        float knockbackDistance = 1f * multiply; 
        float knockbackDuration = 0.2f; 

        StartCoroutine(KnockbackEffect(knockbackDirection, knockbackDistance, knockbackDuration));
    }

    IEnumerator KnockbackEffect(Vector3 direction, float distance, float duration) {
        float elapsed = 0f;

        while(elapsed < duration) {
            float step = distance * (Time.deltaTime / duration);
            transform.position += direction * step;

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    public int getHealth() { return this.health; }
}