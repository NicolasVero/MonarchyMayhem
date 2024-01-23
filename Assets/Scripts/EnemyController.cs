using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    
    private Transform player;
    private NavMeshAgent agent;



    private ParticleSystem collectParticle;
    private Transform agentPos;

    private PlayerController playerController;
    private Animator animator;

    [SerializeField] private WeaponsDropper weaponsDropper;

    private string enemyType;
    private int health;
    private int attack;
    private float attackSpeed;
    private float range;
    private float speed;
    private int xp;

    private bool canMove = true, canAttack = true, isAlive = true, deathCount = false;
    private float timeSinceLastAttack;
    private System.Random random = new System.Random();


    private void Awake() {

        this.enemyType = EnemyController.getEnemyType(gameObject.name);
        TextAsset enemiesStats = Resources.Load<TextAsset>("Data/EnemiesStats");

        if(enemiesStats != null) {
            EnemiesStats enemiesStatsData  = JsonUtility.FromJson<EnemiesStats>(enemiesStats.text);
            EnemyStats enemy = Array.Find(enemiesStatsData.enemiesStat, e => e.type == this.enemyType);

            this.health      = enemy.health;
            this.attack      = enemy.attack;
            this.attackSpeed = enemy.attackSpeed;
            this.range       = enemy.range;
            this.speed       = enemy.speed;
            this.xp          = enemy.xp;
        }


        this.agent = GetComponent<NavMeshAgent>();
        this.player = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.collectParticle = this.GetComponentInChildren<ParticleSystem>();
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.animator = GetComponentInChildren<Animator>();

        this.agentPos = this.agent.transform;
        this.agent.speed = this.speed;
    }

    private static string getEnemyType(string name) {
       return name.Split('_')[0];
    }

    private void FixedUpdate() {

        // this.weaponsDropper.CreateWeapon(this.GiveRandomWeaponID(), transform.position);


        if(isAlive) {

            resetAnims();

            // this.agentPos.transform.LookAt(this.player);
            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.player){
                this.agent.destination = player.position;
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

            if(!this.deathCount) {
                this.playerController.incrementKillCounter();
                this.playerController.incrementStatCounter();
                this.deathCount = true;
            }
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
        this.playerController.TakeDamage(this.attack);
    }

    void resetAnims(){
        this.animator.SetBool("Idle", false);
        this.animator.SetBool("Walk", false);
    }

    void Death() {
        this.animator.SetInteger("Death", UnityEngine.Random.Range(1, 4));
        canMove = false;
        canAttack = false;
        isAlive = false;

        StartCoroutine(DestroyEnemy(2f));
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

        int resultat = random.Next(1, this.weaponsDropper.GetWeaponsListLength() + 1);

        return (random.NextDouble() < probabilite) ? resultat : GiveRandomWeaponID();
    }

    void ActivateCollectParticle(){
        this.collectParticle.Play();
    }
}
