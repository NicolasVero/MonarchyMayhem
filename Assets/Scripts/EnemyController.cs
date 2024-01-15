using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    
    private ParticleSystem collectParticle;
    private Transform player;
    private PlayerController playerController;
    private NavMeshAgent agent;
    private Transform agentPos;
    private Animator animator;
    private int health = 20;
    

    private float attackSpeed = 2f;
    private float speed = 1;

    private bool canMove = true, canAttack = true, isAlive = true;
    private float timeSinceLastAttack;

    private void Awake() {
        this.agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.collectParticle = this.GetComponentInChildren<ParticleSystem>();
        this.player = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.agentPos = this.agent.transform;
        this.animator = GetComponentInChildren<Animator>();
        this.agent.speed = speed;
    }

    private void FixedUpdate() {

        if(isAlive) {

            resetAnims();

            this.agentPos.transform.LookAt(this.player);
            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.player){
                this.agent.SetDestination(this.player.position);
            }

            if(this.player && this.canMove){
                if(Vector3.Distance(this.player.position, this.agentPos.position) <= this.agent.stoppingDistance){
                    this.animator.SetBool("Idle", true);

                    if(this.timeSinceLastAttack >= this.attackSpeed && this.canAttack){
                        this.Attack();
                        this.timeSinceLastAttack = 0f;
                    }
                } else {
                    this.Move();
                }
            }
        }
    }
    
    public void TakeDamage(int damage){

        this.health -= damage;

        if(this.health <= 0) {
            this.Death();
            this.playerController.incrementKillCounter();
        }
    }

    public void ApplyKnockback(float multiply) {
        Vector3 knockbackDirection = -transform.forward;
        float knockbackDistance = 1f * multiply; 
        float knockbackDuration = 0.2f; 

        // StartCoroutine(DelayedKnockbackEffect(knockbackDirection, knockbackDistance, knockbackDuration));
        StartCoroutine(KnockbackEffect(knockbackDirection, knockbackDistance, knockbackDuration));
    }

    IEnumerator DelayedKnockbackEffect(Vector3 direction, float distance, float duration) {
        yield return new WaitForSeconds(0f);
        StartCoroutine(KnockbackEffect(direction, distance, duration));
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
        this.animator.SetBool("Walk", true);
    }
    void Attack(){
        this.ActivateCollectParticle();
        this.animator.SetTrigger("Attack");
        this.playerController.TakeDamage(2);
    }

    void resetAnims(){
        this.animator.SetBool("Idle", false);
        this.animator.SetBool("Walk", false);
    }

    void Death(){
        this.animator.SetInteger("Death", UnityEngine.Random.Range(1, 4));
        canMove = false;
        canAttack = false;
        isAlive = false;

        StartCoroutine(DestroyEnemy(2f));
    }

    private IEnumerator DestroyEnemy(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        // this.playerController.XPGain(1);
    }

    void ActivateCollectParticle(){
        this.collectParticle.Play();
    }
}