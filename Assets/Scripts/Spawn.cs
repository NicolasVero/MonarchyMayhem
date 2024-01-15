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


    [SerializeField] GameObject newObject;
    [SerializeField] Vector3 position;

    void Start() {

        
        Instantiate(newObject, position, Quaternion.identity);
    }

    void Update() {
    }
}