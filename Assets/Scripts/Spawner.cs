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



    void Awake(){

        if (currentPickUpGroup == null || currentPickUpGroup.Length == 0) {
            GeneratePickUpGroup();
        }
    }

    void Update() {

        if(this.isActive && !this.isPaused) {
            this.timer += Time.deltaTime;

            if(this.timer >= this.spawnDelay) {
                this.timer = 0f;
                SpawnEnemies();
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
        if (currentPickUpIndex >= currentPickUpGroup.Length) {
            currentPickUpIndex = 0;
        }

        GameObject pickupToSpawn = currentPickUpGroup[currentPickUpIndex];
        Instantiate(pickupToSpawn, transform.position, Quaternion.identity).transform.parent = this.collectiblesContainer.transform; 

    }


    public void ActiveSpawnerPickUp() {
        this.allowPickUp = true;
        SpawnPickUp();
    }

    private void GeneratePickUpGroup() {
        currentPickUpGroup = new GameObject[pickUps.Length];
        for(int i = 0; i < currentPickUpGroup.Length; i++) 
            currentPickUpGroup[i] = pickUps[i % pickUps.Length];
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
}