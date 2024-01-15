using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    [Header("Spawnable")]
    [SerializeField] bool allowPeasants;
    [SerializeField] bool allowBourgeois;
    [SerializeField] bool allowKnights;

    [Header("Chance of occurrence")]
    [SerializeField] float chancePeasants;
    [SerializeField] float chanceBourgeois;
    [SerializeField] float chanceKnights;

    [Header("Location")]
    [SerializeField] Vector3 position;

    [Header("Spawn parameters")]
    [SerializeField] private float spawnDelay;


    [Header("")]
    [SerializeField] GameObject prefab;



    private float timer = 0f;

    void Start() {

        prefab = Resources.Load<GameObject>("Characters/Enemies/Peasants/Peasant_3/peasant_3");
        Debug.Log(prefab);
        Instantiate(prefab, position, Quaternion.identity);
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
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}