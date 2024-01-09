using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAiController : MonoBehaviour {
    
    private Transform player;
    private PlayerController playerController;
    private NavMeshAgent agent;
    private Transform agentPos;
    private Animator _animator;
    private int health = 20;
    
    private ParticleSystem collectParticle;
    private float delayActivation = 0.3f;
    private float cooldownTime = 1.0f; // Temps de récupération entre chaque activation
    private float lastActivationTime = 0f;

    private float attackSpeed = 2f;

    private bool canMove = true, canAttack = true, isAlive = true;
    private float timeSinceLastAttack;

    private void Awake() {
        this.agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.collectParticle = this.GetComponentInChildren<ParticleSystem>();
        this.player = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.agentPos = this.agent.transform;
        this._animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() {

        if(isAlive) {

            resetAnims();
            // Look at the player at all times
            this.agentPos.transform.LookAt(this.player);

            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.player){
                this.agent.SetDestination(this.player.position);
            }

            if (this.player && this.canMove){
                // Attack
                // Check distance -> if close enough, stop
                if (Vector3.Distance(this.player.position, this.agentPos.position) <= this.agent.stoppingDistance){
                    this._animator.SetBool("Idle", true);
                    // Check if can attack
                    if (this.timeSinceLastAttack >= this.attackSpeed && this.canAttack){
                        this.Attack();
                        this.timeSinceLastAttack = 0f;
                    }
                }
                // else Move
                else{
                    this.Move();
                }
            }
        }
    }
    
    public void TakeDamage(int damage){

        this.health -= damage;
        Debug.Log("degats pris : " + this.health);

        if(this.health <= 0) {
            this.Death();
            // Destroy(this.gameObject);
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





        void Move(){
        // Move towards the player
        this._animator.SetBool("Walk", true);
        // this.agent.SetDestination(this.player.position);
    }
    void Attack(){
        Debug.Log("Attack");
        this.ActivateCollectParticle();
        this._animator.SetTrigger("Attack");
    }


    void resetAnims(){
        // Resets
        this._animator.SetBool("Idle", false);
        this._animator.SetBool("Walk", false);
    }

    // OnDeath : canMove = false, canAttack = false
    void Death(){
        this._animator.SetInteger("Death", UnityEngine.Random.Range(1, 4));
        canMove = false;
        canAttack = false;
        isAlive = false;

        StartCoroutine(DestroyEnemy(2f));
    }

    private IEnumerator DestroyEnemy(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void ActivateCollectParticle(){
        collectParticle.Play();
    }
}