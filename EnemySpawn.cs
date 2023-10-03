using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] spawnPoint;
    public GameObject[] enemyObject;

    private GameObject[] verifEnemy;

    private int randomSpawnSpot;
    private int randomEnemy;

    private PlayerController pc;
    
    private float spawnTime = 2f;
    float waveBreak = 20f;

    private int enemyCount;
    private int compteur;
    private int waveNumber = 0;
    
    private bool waveInProgress = false;
    
    void Awake(){
        pc = GameObject.Find("Knight").GetComponent<PlayerController>();
    }

    void FixedUpdate(){
        if (!this.waveInProgress){
            ResetWave();
            this.waveInProgress = true;
        }
        verifEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (verifEnemy.Length==4 && this.waveInProgress)
            StartCoroutine(Break());
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
            Debug.Log(verifEnemy.Length);
        }
        Debug.Log("Vague Terminée");
    }

   	private IEnumerator Break(){
        Debug.Log("Début de la pause");
        yield return new WaitForSeconds(this.waveBreak);
        this.waveInProgress = false;
        Debug.Log("Pause terminée");
    }

}
