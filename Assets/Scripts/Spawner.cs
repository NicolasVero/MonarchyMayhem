using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [Header("Spawnable")]
    [SerializeField] bool allowPeasants;
    [SerializeField] bool allowBourgeois;
    [SerializeField] bool allowKnights;

    [Header("Chance of spawn")]
    [SerializeField] float chancePeasants;
    [SerializeField] float chanceBourgeois;
    [SerializeField] float chanceKnights;

    [Header("Spawn parameters")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject enemiesContainer;

    [Header("Instances")]
    [SerializeField] GameObject[] peasants;
    [SerializeField] GameObject[] bourgeois;
    [SerializeField] GameObject[] knights;

    private System.Random random = new System.Random();
    private float timer = 0f;


    void Update() {
        this.timer += Time.deltaTime;

        if(this.timer >= this.spawnDelay) {
            this.timer = 0f;
            SpawnEnemies();
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
}