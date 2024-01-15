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
    // [SerializeField] GameObject k1;
    // [SerializeField] GameObject k2;
    private GameObject[] peasants;
    private GameObject[] bourgeois;

    private System.Random random = new System.Random();

    private float timer = 0f;

    void Start() {
        peasants = new GameObject[] {p1,p2,p3,p4,p5};
        bourgeois = new GameObject[]{b1,b2,b3,b4};

        // prefab = Resources.Load<GameObject>("Characters/Enemies/Peasants/Peasant_1/peasant_1");
        // GameObject instantiatedObject = Instantiate(prefab);
    }

    void Update() {
        timer += Time.deltaTime;

        if(timer >= spawnDelay) {
            timer = 0f;
            SpawnEnemies();
        }
    }

    private void SpawnEnemies() {
        if(allowPeasants) {
            Instantiate(peasants[random.Next(0, peasants.Length - 1)], transform.position, Quaternion.identity);
        }

        if(allowBourgeois) {
            Instantiate(bourgeois[random.Next(0, bourgeois.Length - 1)], transform.position, Quaternion.identity);
        }
    }
}