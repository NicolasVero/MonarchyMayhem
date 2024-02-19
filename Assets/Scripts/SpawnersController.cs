using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnersController : MonoBehaviour {
    
    [Header("Spawners")]
    [SerializeField] private int maxActive;
    [SerializeField] private float radius;
    [SerializeField] private int maxEntities;
    [SerializeField] private GameObject enemiesContainer;
    
    private bool isPaused = false;
    private GameObject[] spawnerObjects;
    private Difficulty difficultyController;

    void Start(){

        this.difficultyController = FindObjectOfType<Difficulty>();
        this.difficultyController.DisableChoice();

        if(this.difficultyController.GetDifficulty() == "easy") this.maxEntities = 10;
        if(this.difficultyController.GetDifficulty() == "medium") this.maxEntities = 20;
        if(this.difficultyController.GetDifficulty() == "hard") this.maxEntities = 40;
        
    
        List<GameObject> spawnerList = new List<GameObject>();
        foreach(Transform child in transform) {
            if(child.GetComponent<Spawner>() != null) {
                spawnerList.Add(child.gameObject);
            }
        }

        this.spawnerObjects = spawnerList.ToArray();

        if(this.maxActive > this.spawnerObjects.Length)
            this.maxActive = this.spawnerObjects.Length;

        this.ActivateRandomSpawners();
    }

    void Update() {

        if(this.enemiesContainer.transform.childCount >= maxEntities) {
            if(!this.isPaused) 
                this.PauseSpawners();
        } else {
            if(this.isPaused) 
                this.ResumeSpawners();
        }
    }

    private void ActivateRandomSpawners() {
        List<int> indices = new List<int>();

        for(int i = 0; i < this.spawnerObjects.Length; i++) {
            indices.Add(i);
        }

        for(int i = 0; i < maxActive; i++) {
            int randomIndex = GameController.Random(0, indices.Count - 1);
            int spawnerIndex = indices[randomIndex];
            this.spawnerObjects[spawnerIndex].GetComponent<Spawner>().ActiveSpawner();
            indices.RemoveAt(randomIndex);
        }
    }

    public void PauseSpawners() {
        foreach(GameObject spawnerObject in this.spawnerObjects) {
            spawnerObject.GetComponent<Spawner>().PauseSpawner();
        }
        this.isPaused = true;
    }

    public void ResumeSpawners() {
        foreach(GameObject spawnerObject in this.spawnerObjects) {
            spawnerObject.GetComponent<Spawner>().ResumeSpawner();
        }
        this.isPaused = false;
    }

    public void SetMaxEntities(int maxEntities) {
        this.maxEntities = maxEntities;
    } 
}
