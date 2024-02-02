using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersController : MonoBehaviour {
    
    [Header("Spawners")]
    [SerializeField] private int maxActive;
    [SerializeField] private float radius;
    [SerializeField] private int maxEntities;
    private GameObject[] spawnerObjects;


    void Start(){
    
        List<GameObject> spawnerList = new List<GameObject>();
        foreach (Transform child in transform) {
            if (child.GetComponent<Spawner>() != null) {
                spawnerList.Add(child.gameObject);
            }
        }
        spawnerObjects = spawnerList.ToArray();

        if(this.maxActive > spawnerObjects.Length)
            this.maxActive = spawnerObjects.Length;

        ActivateRandomSpawners();
    }

    void Update() {

    }

    private void ActivateRandomSpawners() {
        List<int> indices = new List<int>();

        for (int i = 0; i < spawnerObjects.Length; i++) {
            indices.Add(i);
        }

        for (int i = 0; i < maxActive; i++) {
            int randomIndex = Random.Range(0, indices.Count);
            int spawnerIndex = indices[randomIndex];
            spawnerObjects[spawnerIndex].GetComponent<Spawner>().ActiveSpawner();
            indices.RemoveAt(randomIndex);
        }
    }
}
