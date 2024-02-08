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
    private System.Random random = new System.Random();
    private float timer = 0f;
    private int currentPickUpIndex = 0;
    private GameObject[] currentPickUpGroup;
    private int pickUpsGroupSize = 2;



    void Awake(){
            if (currentPickUpGroup == null || currentPickUpGroup.Length == 0) {
            // Si le groupe actuel est nul ou vide, générez un nouveau groupe de pickups
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

        int randNumber = this.random.Next(0, 100);

        if(this.allowPeasants) {
            if(randNumber < this.chancePeasants) {
                Instantiate(this.peasants[this.random.Next(0, this.peasants.Length - 1)], transform.position, Quaternion.identity).transform.parent = this.enemiesContainer.transform;
            }
        }

        if(this.allowBourgeois) {
            if(randNumber < this.chanceBourgeois) {
                Instantiate(this.bourgeois[this.random.Next(0, this.bourgeois.Length - 1)], transform.position, Quaternion.identity).transform.parent = this.enemiesContainer.transform;
            }
        }

        if(this.allowKnights) {
            if(randNumber < this.chanceKnights) {
                Instantiate(this.knights[this.random.Next(0, this.knights.Length - 1)], transform.position, Quaternion.identity).transform.parent = this.enemiesContainer.transform;
            }
        }
    }

    private void SpawnPickUp() {
        // Vérifier si l'index dépasse la taille du tableau de pickUps du groupe
        if (currentPickUpIndex >= currentPickUpGroup.Length) {
            currentPickUpIndex = 0; // Revenir au début du groupe
        }

        // Instancier le pickup correspondant à l'index actuel du groupe
        GameObject pickupToSpawn = currentPickUpGroup[currentPickUpIndex];
        Instantiate(pickupToSpawn, transform.position, Quaternion.identity).transform.parent = this.collectiblesContainer.transform; 

    }


    public void ActiveSpawnerPickUp() {
        this.allowPickUp = true;
        SpawnPickUp();
    }

    private void GeneratePickUpGroup() {
        // Remplir le groupe de pickups avec les pickups dans l'ordre du tableau
        currentPickUpGroup = new GameObject[pickUpsGroupSize];
        for (int i = 0; i < pickUpsGroupSize; i++) {
            currentPickUpGroup[i] = pickUps[i % pickUps.Length]; // Utiliser l'opérateur modulo pour boucler à travers le tableau de pickups
        }
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