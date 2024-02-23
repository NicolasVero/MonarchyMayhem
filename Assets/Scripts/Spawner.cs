using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [Header("Spawnable")]
    [SerializeField] bool allowPeasants;
    [SerializeField] bool allowBourgeois;
    [SerializeField] bool allowKnights;

    [Header("Chance of spawn")]
    [Range(0, 100)][SerializeField] float chancePeasants;
    [Range(0, 100)][SerializeField] float chanceBourgeois;
    [Range(0, 100)][SerializeField] float chanceKnights;

    [Header("Spawn parameters")]
    [SerializeField] bool viewSpawnPoint;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject enemiesContainer;
    [SerializeField] private GameObject collectiblesContainer;

    [Header("Instances")]
    [SerializeField] GameObject[] peasants;
    [SerializeField] GameObject[] bourgeois;
    [SerializeField] GameObject[] knights;
    [SerializeField] GameObject[] pickUps;

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
        
        if(this.currentPickUpGroup == null || this.currentPickUpGroup.Length == 0) {
            this.GeneratePickUpGroup();
        }
    }

    void Start() {
        this.player = GameObject.FindWithTag(Names.MainCharacter).GetComponent<PlayerController>();
        this.spawnerController = GetComponentInParent<SpawnersController>();
        this.radius = this.spawnerController.GetRadius();
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