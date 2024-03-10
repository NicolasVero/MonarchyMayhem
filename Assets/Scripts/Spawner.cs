using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


    private bool allowPeasants;
    private bool allowBourgeois;
    private bool allowKnights;

    private int chancePeasants;
    private int chanceBourgeois;
    private int chanceKnights;

    private bool viewSpawnPoint;
    private float spawnDelay;

    private GameObject[] peasants;
    private GameObject[] bourgeois;
    private GameObject[] knights;
    private GameObject[] pickUps;


    private GameObject enemiesContainer;
    private GameObject collectiblesContainer;
    private bool isActive = false, isPaused = false, allowPickUp;
    private float timer = 0f;
    private int currentPickUpIndex = 0;
    private GameObject[] currentPickUpGroup;
    private Difficulty difficultyController;
    private SpawnersController spawnerController;
    private float radius;
    private PlayerController player;


    void Awake(){

        GameObject spawn = transform.Find("Spawn").gameObject;
        spawn.SetActive(viewSpawnPoint);


        this.difficultyController = FindObjectOfType<Difficulty>();
        this.difficultyController.DisableChoice();

        if(this.difficultyController.GetDifficulty() == "easy") this.spawnDelay = 5;
        if(this.difficultyController.GetDifficulty() == "medium") this.spawnDelay = 4;
        if(this.difficultyController.GetDifficulty() == "hard") this.spawnDelay = 2;
    }

    void Start() {
        this.player = GameObject.FindWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.spawnerController = GetComponentInParent<SpawnersController>();
        this.radius = this.spawnerController.GetRadius();

        this.allowPeasants = this.spawnerController.GetAllowPeasant();
        this.allowBourgeois = this.spawnerController.GetAllowBouregois();
        this.allowKnights = this.spawnerController.GetAllowKnights();

        this.chancePeasants = this.spawnerController.GetChancePeasants();
        this.chanceBourgeois = this.spawnerController.GetChanceBourgeois();
        this.chanceKnights = this.spawnerController.GetChanceKnights();
    
        this.enemiesContainer = this.spawnerController.GetEnemiesContainer();
        this.collectiblesContainer = this.spawnerController.GetColleciblesContainer();
    
        this.viewSpawnPoint = this.spawnerController.GetViewSpawnPoint();
        this.spawnDelay = this.spawnerController.GetSpawnDelay();
    
        this.peasants = this.spawnerController.GetPeasantsPrefabs();
        this.bourgeois = this.spawnerController.GetBourgeoisPrefabs();
        this.knights = this.spawnerController.GetKnightsPrefabs();
        this.pickUps = this.spawnerController.GetPickUpsPrefabs();

        if(this.currentPickUpGroup == null || this.currentPickUpGroup.Length == 0) {
            this.GeneratePickUpGroup();
        }
    }

    void Update() {

        if(this.isActive && !this.isPaused && !this.IsPlayerInRadius()) {
            this.timer += Time.deltaTime;

            if(this.timer >= this.spawnDelay) {
                this.timer = 0f;
                this.SpawnEnemies();
            }
        }
    }
    
    private void SpawnEnemies() {

        int randNumber = GameController.Random(0, 100);

        if(this.allowPeasants) {
            if(randNumber < this.chancePeasants) {
                Instantiate(this.peasants[GameController.Random(0, this.peasants.Length - 1)], transform.position, Quaternion.identity).transform.parent = this.enemiesContainer.transform;
            }
        }

        if(this.allowBourgeois) {
            if(randNumber < this.chanceBourgeois) {
                Instantiate(this.bourgeois[GameController.Random(0, this.bourgeois.Length - 1)], transform.position, Quaternion.identity).transform.parent = this.enemiesContainer.transform;
            }
        }

        if(this.allowKnights) {
            if(randNumber < this.chanceKnights) {
                Instantiate(this.knights[GameController.Random(0, this.knights.Length - 1)], transform.position, Quaternion.identity).transform.parent = this.enemiesContainer.transform;
            }
        }
    }

    private void SpawnPickUp() {
        if(this.currentPickUpIndex >= this.currentPickUpGroup.Length) {
            this.currentPickUpIndex = 0;
        }

        GameObject pickupToSpawn = this.currentPickUpGroup[this.currentPickUpIndex];
        Vector3 spawnPosition = transform.position + Vector3.up * 1.0f;
        Instantiate(pickupToSpawn, spawnPosition, Quaternion.identity).transform.parent = this.collectiblesContainer.transform;
    }



    public void ActiveSpawnerPickUp() {
        this.allowPickUp = true;
        SpawnPickUp();
    }

    private void GeneratePickUpGroup() {
        this.currentPickUpGroup = new GameObject[this.pickUps.Length];
        for(int i = 0; i < this.currentPickUpGroup.Length; i++) 
            this.currentPickUpGroup[i] = this.pickUps[i % this.pickUps.Length];
    }

    public void ActiveSpawner() {
        this.isActive = true;
    }

    public void PauseSpawner() {
        this.isPaused = true;
    }

    public void ResumeSpawner() {
        this.isPaused = false;
    }

    public void IncrementIndex(){
        this.currentPickUpIndex++;
    }

    public bool IsPlayerInRadius() {
        return Vector3.Distance(this.player.transform.position, transform.position) <= this.radius;
    }
}