using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour {

    [SerializeField] private SpawnersController spawnersController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject smokeGO;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private Material darkSkin;

    private Transform playerPosition;
    private Transform enemyPosition;
    private ParticleSystem smokeEffect;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    private PlayerController playerController;
    private AudioController audio;
    private Difficulty difficultyController; 

    private string walkMethod;
    private int attack, health, maxHealth, xp;
    private float chanceToDrop, attackSpeed, range, speed, timeSinceLastAttack, sliderVelocity = 0.0f, attackDelay = 0.5f;
    private bool canMove = true, canAttack = true, isAlive = true, firstPhasePassed = false, cooldown = false, isInTransition = false, bossRegen = false, isWaitingToAttack = false;

    void Start() {
        this.spawnersController.SetMaxEntities(0);
        this.walkMethod = "Walk";
        this.bossName.text = "Nabil";
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

        this.audio.StopThemeSFX();
        this.audio.PlayBossThemeSFX(0);
    }

    private void LoadBossStats() {
        TextAsset enemiesStats = Resources.Load<TextAsset>("Data/EnemiesStats");
        string phase = !this.firstPhasePassed ? "boss_1" : "boss_2";
        DifficultyStats difficultyStats = null;

        if(enemiesStats != null) {
            EnemiesStats enemiesStatsData  = JsonUtility.FromJson<EnemiesStats>(enemiesStats.text);
            this.difficultyController = FindObjectOfType<Difficulty>();
            string difficulty = difficultyController.GetDifficulty();

            if(difficulty == "easy"  ) difficultyStats = enemiesStatsData.easy;
            if(difficulty == "medium") difficultyStats = enemiesStatsData.medium;
            if(difficulty == "hard"  ) difficultyStats = enemiesStatsData.hard;
        }

        if(difficultyStats != null) {
            EnemyStats enemy = Array.Find(difficultyStats.enemiesStat, e => e.type == phase);

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

        if (this.bossRegen) {
            this.BossRegen();
        }        

        if(this.isAlive) {
            
            ResetAnims();
            if (!this.isInTransition)
                this.enemyPosition.LookAt(playerPosition);

            this.timeSinceLastAttack += Time.fixedDeltaTime;

            if(this.playerPosition && this.canMove) {
                if(Vector2.Distance(new Vector2(playerPosition.position.x, playerPosition.position.z), new Vector2(enemyPosition.position.x, enemyPosition.position.z)) <= this.navMeshAgent.stoppingDistance) {
                    this.animator.SetBool("Idle", true);
                    
                    if(this.timeSinceLastAttack >= this.attackSpeed && this.canAttack && !this.isWaitingToAttack){
                        StartCoroutine(WaitAndAttack());
                    }
                }
                else {
                    this.Move();
                }
            }
        }
    }

    private IEnumerator WaitAndAttack() {
        this.isWaitingToAttack = true;
        yield return new WaitForSeconds(this.attackDelay);
        if(Vector2.Distance(new Vector2(playerPosition.position.x, playerPosition.position.z), new Vector2(enemyPosition.position.x, enemyPosition.position.z)) <= this.navMeshAgent.stoppingDistance && !this.isInTransition) {
            this.Attack();
            this.timeSinceLastAttack = 0f;
        }
        this.isWaitingToAttack = false;
    }

    private void Move() {
        this.navMeshAgent.destination = playerPosition.position;
        this.animator.SetBool(this.walkMethod, true);
    }
    
    private void Attack() {
        if(this.playerController.IsAlive()) {
            this.animator.SetTrigger("Attack");
            this.audio.PlaySlashSFX();
            this.playerController.TakeDamage(this.attack);
        }
    }

    public void ApplyDamage() {
        Invoke("TakeDamage", (this.playerController.GetWeaponAttackSpeed() + this.playerController.GetAttackSpeed()) / 5);
    }

    public void TakeDamage() {

        if(!this.cooldown) {
            this.health -= this.playerController.GetAttack() + this.playerController.GetWeaponAttack();
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
        this.animator.SetBool("Walk", false);
        this.animator.SetBool("Sprint", false);
    }

    private void Death() {        
        foreach(Transform child in weaponHolder.transform) {
            child.gameObject.SetActive(false);
        }
        this.animator.SetBool("Death", true);
        Invoke("Destroyer", 5f);
    }

    private void StopMovement() {
        this.canMove = false;
        this.canAttack = false;
        this.isAlive = false;
    }
    private void StartMovement() {
        this.canMove = true;
        this.canAttack = true;
        this.isAlive = true;
    }

    private void StartPhaseTwo() {
        this.tag = "Untagged";
        this.firstPhasePassed = true;
        this.isInTransition = true;

        this.animator.SetBool("Walk", false);
        this.walkMethod = "Sprint";

        this.LoadBossStats();
        this.animator.SetBool("TransitionSecondPhase", true);

        Invoke("Camouflage", 2.5f);
        Invoke("ChangeSkin", 5f);
        Invoke("PhaseTwo", 10f);

    }

    private void PhaseTwo() {
        this.tag = "Boss";
        this.isInTransition = false;
        this.bossRegen = false;
        
        this.audio.StopBossThemeSFX();
        this.audio.PlayBossThemeSFX(1);

        this.StartMovement();

        this.spawnersController.SetMaxEntities(15);
    }

    private void ChangeSkin() {
        this.animator.SetBool("TransitionSecondPhase", false);
        this.bossRegen = true;
        this.bossName.text = "Dark Nabil";

        foreach(Transform child in weaponHolder.transform) {
            if(child.gameObject.name == "weapon_3") {
                child.gameObject.SetActive(true);
            } else {
                child.gameObject.SetActive(false);
            }
        }
        GameObject.Find("BossSkin").GetComponent<Renderer>().material = darkSkin;
    }

    private void Camouflage() {
        Instantiate(smokeGO, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.identity);
        GameObject.FindGameObjectWithTag("Smoke").GetComponent<ParticleSystem>().Play();

        this.SetHealthBarMax(this.maxHealth);
    }

    private void SetHealthBar(int hp) { 
        this.healthBar.value = hp; 
    }

    private void SetHealthBarMax(int max) { 
        this.healthBar.maxValue = max; 
    }

    private void BossRegen() {
        float currentHealth = Mathf.MoveTowards(this.healthBar.value, this.maxHealth, 300 * Time.deltaTime);
        this.healthBar.value = currentHealth;
    }

    public int GetHealth() { 
        return this.health;          
    }
    
    public int GetMaxHealth() {
        return this.maxHealth;
    }   

    private void Destroyer() {
        Destroy(GameObject.FindGameObjectWithTag(Names.MainCharacter));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        Destroy(GameObject.FindGameObjectWithTag("Difficulty"));
        this.ChangeScene();
    } 

    private void ChangeScene() {
        SceneManager.LoadScene(Names.CinematicScenes[1], LoadSceneMode.Single);
    }
}
