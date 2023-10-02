using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    EnemyAiController enemy;
    PlayerController player;
    void Awake(){
        player = GameObject.Find("Knight").GetComponent<PlayerController>();
    }
    // enemy = GameObject.Find("Zombie").GetComponent<EnemyAiController>();
    // enemy.AttackEnemy(attack);
    void OnTriggerEnter(Collider c){
        if (player.canAttack){
            if (c.gameObject.CompareTag("Enemy")){
                c.gameObject.GetComponent<EnemyAiController>().AttackEnemy(10);
            }
        }
    }
}
