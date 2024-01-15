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
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    [SerializeField] GameObject p3;
    [SerializeField] GameObject p4;
    [SerializeField] GameObject p5;
    [SerializeField] GameObject b1;
    [SerializeField] GameObject b2;
    [SerializeField] GameObject b3;
    [SerializeField] GameObject b4;
    [SerializeField] GameObject k1;
    [SerializeField] GameObject k2;
    private GameObject[] peasants;
    private GameObject[] bourgeois;
    private GameObject[] knights;

    private System.Random random = new System.Random();

    private float timer = 0f;

    void Start() {
        this.peasants = new GameObject[] {
            p1, p2, p3, p4, p5
        };

        this.bourgeois = new GameObject[] {
            b1, b2, b3, b4
        };
        
        this.knights = new GameObject[] {
            k1, k2
        };

        // prefab = Resources.Load<GameObject>("Characters/Enemies/Peasants/Peasant_1/peasant_1");
        // GameObject instantiatedObject = Instantiate(prefab);
    }

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