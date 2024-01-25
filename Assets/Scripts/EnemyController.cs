using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    
    public Transform playerPosition;
    public Transform enemyPosition;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public ParticleSystem attackParticle;
    public PlayerController playerController;

    [SerializeField] private WeaponsDropper weaponsDropper;

    private string enemyType;
    private int attack, health, xp;
    private float attackSpeed, range, speed, timeSinceLastAttack;
    private bool canMove = true, canAttack = true, isAlive = true, deathCount = false;

    private System.Random random = new System.Random();

    private void Awake() {

        this.enemyType = EnemyController.GetEnemyType(gameObject.name);
        TextAsset enemiesStats = Resources.Load<TextAsset>("Data/EnemiesStats");

        if(enemiesStats != null) {
            EnemiesStats enemiesStatsData  = JsonUtility.FromJson<EnemiesStats>(enemiesStats.text);
            EnemyStats enemy = Array.Find(enemiesStatsData.enemiesStat, e => e.type == this.enemyType);

            this.attack      = enemy.attack;
            this.attackSpeed = enemy.attackSpeed;
            this.health      = enemy.health;
            this.range       = enemy.range;
            this.speed       = enemy.speed;
            this.xp          = enemy.xp;
        }

        this.playerPosition = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        this.enemyPosition = this.navMeshAgent.transform;
        
        this.attackParticle = this.GetComponentInChildren<ParticleSystem>();
        this.animator = GetComponentInChildren<Animator>();
        
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.weaponsDropper = GameObject.FindGameObjectWithTag("WeaponsDropper").GetComponent<WeaponsDropper>();
        this.navMeshAgent.speed = this.speed;
    }

    private void FixedUpdate() {

        if(isAlive) {

            ResetAnims();

            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.playerPosition) {
                this.navMeshAgent.destination = playerPosition.position;
            }

            if(this.playerPosition && this.canMove){
                if(Vector3.Distance(this.playerPosition.position, this.enemyPosition.position) <= this.navMeshAgent.stoppingDistance){
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
    
    private void Move() {
        this.animator.SetBool("Walk", true);
    }
    
    private void Attack() {
        this.ActivateCollectParticle();
        this.animator.SetTrigger("Attack");
        this.playerController.TakeDamage(this.attack);
    }

    public void TakeDamage(int damage) {

        this.health -= damage;

        if(this.health <= 0) {
            this.Death();

            if(!this.deathCount) {
                this.playerController.incrementKillCounter();
                this.playerController.incrementStatCounter();
                this.deathCount = true;
            }
        }
    }

    private void ResetAnims(){
        this.animator.SetBool("Idle", false);
        this.animator.SetBool("Walk", false);
    }

    private void Death() {
        this.animator.SetInteger("Death", UnityEngine.Random.Range(1, 4));
        canMove = false;
        canAttack = false;
        isAlive = false;

        StartCoroutine(DestroyEnemy(2f));
    }

    public void ApplyKnockback(float multiply) {
        Vector3 knockbackDirection = -transform.forward;
        float knockbackDistance = 1f * multiply; 
        float knockbackDuration = 0.2f; 

        StartCoroutine(KnockbackEffect(knockbackDirection, knockbackDistance, knockbackDuration));
    }

    private IEnumerator DelayedKnockbackEffect(Vector3 direction, float distance, float duration) {
        yield return new WaitForSeconds(0f);
        StartCoroutine(KnockbackEffect(direction, distance, duration));
    }

    private IEnumerator KnockbackEffect(Vector3 direction, float distance, float duration) {
        float elapsed = 0f;

        while(elapsed < duration) {
            float step = distance * (Time.deltaTime / duration);
            transform.position += direction * step;

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator DestroyEnemy(float delay) {
        yield return new WaitForSeconds(delay);
        this.weaponsDropper.CreateWeapon(this.GiveRandomWeaponID(), transform.position);
        Destroy(gameObject);
        this.playerController.XPGain(this.xp);
    }

    private int GiveRandomWeaponID() {

        double probabiliteInitiale = 1.0;
        double probabiliteMinimale = 0.1;

        double probabilite = probabiliteInitiale * Mathf.Pow((float)random.NextDouble(), 2);

        int resultat = random.Next(0, this.weaponsDropper.GetWeaponsListLength());

        return (random.NextDouble() < probabilite) ? resultat : GiveRandomWeaponID();
    }

    private void ActivateCollectParticle(){
        this.attackParticle.Play();
    }


    private static string GetEnemyType(string name) {
        return name.Split('_')[0];
    }
}
