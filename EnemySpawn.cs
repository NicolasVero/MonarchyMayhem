using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] spawnPoint;
    public GameObject[] enemyObject;
    private GameObject verif;
    private int randomSpawnSpot;
    private int randomEnemy;

    PlayerController pc;
    
    float spawnTime = 2f;
    float waveBreak = 20f;

    int enemyCount;
    int compteur;
    int waveNumber = 0;
    
    bool waveInProgress = false;
    
    void Awake(){
        pc = GameObject.Find("Knight").GetComponent<PlayerController>();
    }

    void FixedUpdate(){
        if (!this.waveInProgress){
            ResetWave();
            this.waveInProgress = true;
        }
        
    }

    void ResetWave(){
        this.waveNumber += 1;
        this.compteur = 0;
        this.enemyCount = this.waveNumber*5;
        Debug.Log("Début de la vague n°"+this.waveNumber);
        StartCoroutine(Spawn());
    }

   	private IEnumerator Spawn(){
       while (this.compteur < this.enemyCount){
            this.randomSpawnSpot = Random.Range(0, this.spawnPoint.Length);
            this.randomEnemy = Random.Range(0, this.enemyObject.Length);
            Instantiate(this.enemyObject[randomEnemy], this.spawnPoint[randomSpawnSpot].transform.position, Quaternion.identity);
            this.compteur++;
        	yield return new WaitForSeconds(this.spawnTime);
        }
        StartCoroutine(Break());
        Debug.Log("Vague Terminée");
    }

   	private IEnumerator Break(){
        yield return new WaitForSeconds(this.waveBreak);
        this.waveInProgress = false;
        Debug.Log("Pause terminée");
    }

}
