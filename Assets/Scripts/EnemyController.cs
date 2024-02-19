using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    
    private Transform playerPosition;
    private Transform enemyPosition;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private ParticleSystem attackParticle;
    private PlayerController playerController;
    private WeaponsDropper weaponsDropper;
    private QuestController questController;
    private Difficulty difficultyController;

    private string enemyType;
    private int attack, health, xp;
    private float chanceToDrop, attackSpeed, range, speed, timeSinceLastAttack, attackDelay = .5f;
    private bool canMove = true, canAttack = true, isAlive = true, deathCount = false, cooldown = false, isWaitingToAttack = false;

    private void Awake() {

        this.enemyType = EnemyController.GetEnemyType(gameObject.name);
        TextAsset enemiesStats = Resources.Load<TextAsset>("Data/EnemiesStats");
        this.navMeshAgent = GetComponent<NavMeshAgent>();

        if(enemiesStats != null) {
            EnemiesStats enemiesStatsData = JsonUtility.FromJson<EnemiesStats>(enemiesStats.text);
            DifficultyStats difficultyStats = null;
            this.difficultyController = FindObjectOfType<Difficulty>();
            string difficulty = difficultyController.GetDifficulty();

            if(difficulty == "easy") difficultyStats = enemiesStatsData.easy;
            if(difficulty == "medium") difficultyStats = enemiesStatsData.medium;
            if(difficulty == "hard") difficultyStats = enemiesStatsData.hard;
            

            if(difficultyStats != null) {
                EnemyStats enemy = Array.Find(difficultyStats.enemiesStat, e => e.type == this.enemyType);

                this.chanceToDrop = enemy.chanceToDrop;
                this.attack       = enemy.attack;
                this.attackSpeed  = enemy.attackSpeed;
                this.health       = enemy.health;
                this.range        = enemy.range;
                this.speed        = enemy.speed;
                this.xp           = enemy.xp;

                this.navMeshAgent.stoppingDistance = this.range;
            }
        }

        this.playerPosition = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.enemyPosition = this.navMeshAgent.transform;
        this.questController = GameObject.FindGameObjectWithTag("QuestCanvas").GetComponent<QuestController>();

        this.attackParticle = this.GetComponentInChildren<ParticleSystem>();
        this.animator = GetComponentInChildren<Animator>();
        
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.weaponsDropper = GameObject.FindGameObjectWithTag("WeaponsDropper").GetComponent<WeaponsDropper>();
        this.navMeshAgent.speed = this.speed;
    }

    private void FixedUpdate() {

        GameController.DrawCircleAroundObject(transform.position, this.range, 10);

        if(isAlive) {

            ResetAnims();

            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.playerPosition && this.canMove) {
                if(Vector2.Distance(new Vector2(playerPosition.position.x, playerPosition.position.z), new Vector2(enemyPosition.position.x, enemyPosition.position.z)) <= this.navMeshAgent.stoppingDistance) {
                    this.animator.SetBool("Idle", true);

                    if(this.timeSinceLastAttack >= this.attackSpeed && this.canAttack && !this.isWaitingToAttack){
                        StartCoroutine(WaitAndAttack());   
                    }
                } else {
                    this.Move();
                }
            }
        }
    }

    private IEnumerator WaitAndAttack() {
        this.isWaitingToAttack = true;
        yield return new WaitForSeconds(this.attackDelay);
        
        if(Vector2.Distance(new Vector2(playerPosition.position.x, playerPosition.position.z), new Vector2(enemyPosition.position.x, enemyPosition.position.z)) <= this.navMeshAgent.stoppingDistance) {
            this.Attack();
            this.timeSinceLastAttack = 0f;
        }
        
        this.isWaitingToAttack = false;
    }
    
    private void Move() {
        this.animator.SetBool("Walk", true);
        this.navMeshAgent.destination = playerPosition.position;
    }
    
    private void Attack() {
        if(this.playerController.IsAlive()) {

            this.ActivateCollectParticle();
            this.animator.SetTrigger("Attack");
            this.playerController.TakeDamage(this.attack);
        }
    }

    public void ApplyDamage() {
        Invoke("TakeDamage", (this.playerController.GetWeaponAttackSpeed() + this.playerController.GetAttackSpeed()) / 5);
    }

    public void TakeDamage() {

        if(!this.cooldown) { 
            this.health -= this.playerController.GetAttack() + this.playerController.GetWeaponAttack();
            Invoke("DisableCooldown", 1.5f);
            this.cooldown = true;

            if(this.health <= 0) {
                this.Death();

                if(!this.deathCount) {
                    this.playerController.IncrementKillCounter();
                    this.playerController.IncrementStatCounter();
                    questController.UpdateQuestText();
                    this.deathCount = true;
                }
            } else {
                this.ApplyKnockback();
            }
        }
    }

    private void DisableCooldown() {
        this.cooldown = false;
    }

    private void ResetAnims() {
        this.animator.SetBool("Idle", false);
        this.animator.SetBool("Walk", false);
    }

    private void Death() {
        this.animator.SetInteger("Death", GameController.Random(1, 3));
        this.canMove = false;
        this.canAttack = false;
        this.isAlive = false;
        this.gameObject.tag = "Untagged";

        Invoke("DestroyEnemy", 2f);
    }

    private void DestroyEnemy() {

        this.playerController.XPGain(this.xp);

        if(WillDropWeapon())
            this.weaponsDropper.CreateWeapon(transform.position);
        
        Destroy(this.gameObject);
    }

    public void ApplyKnockback() {
        float multiply = playerController.GetKnockback() + playerController.GetWeaponKnockback();
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

    private bool WillDropWeapon() {
        return GameController.RandomFloat() < this.chanceToDrop;
    }

    private void ActivateCollectParticle() {
        this.attackParticle.Play();
    }

    private static string GetEnemyType(string name) {
        return name.Split('_')[0];
    }

    public void Dance() {
        this.animator.SetInteger("Dance", GameController.Random(0, 4));
    }
}
