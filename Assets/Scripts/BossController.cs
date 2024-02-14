using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class BossController : MonoBehaviour {

    [SerializeField] private SpawnersController spawnersController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject smokeGO;
    [SerializeField] private TextMeshProUGUI bossName;

    private Transform playerPosition;
    private Transform enemyPosition;
    private ParticleSystem smokeEffect;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    private PlayerController playerController;
    private AudioController audio;

    private string walkMethod;
    private int attack, health, maxHealth, xp;
    private float chanceToDrop, attackSpeed, range, speed, timeSinceLastAttack;
    private bool canMove = true, canAttack = true, isAlive = true, firstPhasePassed = false, cooldown = false;

    void Start() {
        // Set Max entities -> 0
        this.spawnersController.SetMaxEntities(0);
        this.walkMethod = "Walk";
    }

    private void Awake() {

        this.playerPosition = GameObject.FindGameObjectWithTag(Names.MainCharacter).transform;
        this.navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.audio = GameObject.Find("AudioController").GetComponent<AudioController>();
        this.enemyPosition = this.navMeshAgent.transform;

        LoadBossStats();
        
        this.animator = GetComponentInChildren<Animator>();
        
        this.playerController = GameObject.FindGameObjectWithTag(Names.MainCharacter).GetComponent<PlayerController>();

        
        this.healthBar.maxValue = this.GetHealth();
        this.healthBar.value = this.GetHealth();
        
        this.SetHealthBarMax(this.maxHealth);
    }

    private void LoadBossStats() {
        
        TextAsset enemiesStats = Resources.Load<TextAsset>("Data/EnemiesStats");
        string phase = !this.firstPhasePassed ? "boss_1" : "boss_2";

        if(enemiesStats != null) {
            EnemiesStats enemiesStatsData  = JsonUtility.FromJson<EnemiesStats>(enemiesStats.text);
            EnemyStats enemy = Array.Find(enemiesStatsData.enemiesStat, e => e.type == phase);

            this.chanceToDrop = enemy.chanceToDrop;
            this.attack       = enemy.attack;
            this.attackSpeed  = enemy.attackSpeed;
            this.health       = enemy.health;
            this.maxHealth    = enemy.health;
            this.range        = enemy.range;
            this.speed        = enemy.speed;
            this.xp           = enemy.xp;

            this.navMeshAgent.speed = this.speed;
            this.navMeshAgent.stoppingDistance = this.range;

        }
    }

    private void FixedUpdate() {

        GameController.DrawCircleAroundObject(transform.position, this.range, 10);

        if(isAlive) {

            ResetAnims();

            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.playerPosition && this.canMove) {
                if(Vector3.Distance(this.playerPosition.position, this.enemyPosition.position) <= this.navMeshAgent.stoppingDistance) {
                    this.animator.SetBool("Idle", true);

                    if(this.timeSinceLastAttack >= this.attackSpeed && this.canAttack){
                        this.Attack();
                        this.timeSinceLastAttack = 0f;
                    }
                }
                else {
                    this.Move();
                }
            }
        }
    }

    private void Move() {
        this.navMeshAgent.destination = playerPosition.position;
        this.animator.SetBool(this.walkMethod, true);
    }
    
    private void Attack() {
        if(this.playerController.IsAlive()) {
            this.animator.SetTrigger("Attack");
            this.playerController.TakeDamage(this.attack);
        }
    }

    public void ApplyDamage() {
        Invoke("TakeDamage", (this.playerController.GetWeaponAttackSpeed() + this.playerController.GetAttackSpeed()) / 5);
    }

    public void TakeDamage() {

        if(!this.cooldown) {
            int damage = this.playerController.GetAttack() + this.playerController.GetWeaponAttack();
            this.health -= damage;
            this.SetHealthBar(this.GetHealth());
            Invoke("DisableCooldown", 1.5f);
            this.cooldown = true;

            if(this.health <= 0) {
                if (this.firstPhasePassed)
                    this.Death();
                else
                    this.StartPhaseTwo();
                this.StopMovement();
            }
            else {
                ApplyKnockback();
            }
        }
    }

    private void DisableCooldown() {
        this.cooldown = false;
    }

    public void ApplyKnockback() {
        float multiply = playerController.GetKnockback() + playerController.GetWeaponKnockback();
        Vector3 knockbackDirection = -transform.forward;
        float knockbackDistance = 1f * multiply; 
        float knockbackDuration = 0.2f; 

        StartCoroutine(KnockbackEffect(knockbackDirection, knockbackDistance, knockbackDuration));
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

    private void ResetAnims() {
        this.animator.SetBool("Idle", false);
        this.animator.SetBool(this.walkMethod, false);
    }

    private void Death() {
        this.animator.SetBool("Death", true);
    }

    private void StopMovement() {
        canMove = false;
        canAttack = false;
        isAlive = false;
    }
    private void StartMovement() {
        canMove = true;
        canAttack = true;
        isAlive = true;
    }

    private void StartPhaseTwo() {
        this.tag = "Untagged";
        this.firstPhasePassed = true;

        this.animator.SetBool("Walk", false);
        this.walkMethod = "Sprint";

        this.transform.LookAt(this.playerController.transform);

        this.LoadBossStats();
        this.animator.SetTrigger("TransitionSecondPhase");

        Instantiate(smokeGO, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.identity);
        Invoke("PlaySmoke", 2.5f);
        Invoke("PhaseTwo", 10f);

    }

    private void PhaseTwo() {
        this.tag = "Boss";

        this.audio.StopThemeSFX();
        this.audio.PlayBossThemeSFX();

        this.StartMovement();

        this.spawnersController.SetMaxEntities(150);
        
        this.bossName.text = "Dark Nabil";
        this.SetHealthBarMax(this.maxHealth);
        this.SetHealthBar(this.health);
    }

    private void PlaySmoke() { GameObject.FindGameObjectWithTag("Smoke").GetComponent<ParticleSystem>().Play(); }

    private void SetHealthBar(int hp) { this.healthBar.value = hp; }
    private void SetHealthBarMax(int max) { this.healthBar.maxValue = max; }

    public int GetHealth()               { return this.health;             }
    public int GetMaxHealth()            { return this.maxHealth;          }    


}
