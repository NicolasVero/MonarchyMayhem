using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersController : MonoBehaviour {
    
    [Header("Spawners")]
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private int maxActive;

    void Start(){
        if(this.maxActive > spawners.Length)
            this.maxActive = spawners.Length;

        ActivateRandomSpawners();
    }

    void ActivateRandomSpawners() {
        List<int> indices = new List<int>();

        for (int i = 0; i < spawners.Length; i++) {
            indices.Add(i);
        }

        for (int i = 0; i < maxActive; i++) {
            int randomIndex = Random.Range(0, indices.Count);
            int spawnerIndex = indices[randomIndex];
            spawners[spawnerIndex].GetComponent<Spawner>().ActiveSpawner();
            indices.RemoveAt(randomIndex);
        }
    }
}
