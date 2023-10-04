using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    EnemyAiController enemy;
    PlayerController player;
    void Awake(){
        this.player = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
    }
    // enemy = GameObject.Find("Zombie").GetComponent<EnemyAiController>();
    // enemy.AttackEnemy(attack);
    void OnTriggerEnter(Collider c){
        if (this.player.canAttack){
            if (c.gameObject.CompareTag(Names.BaseEnemy)){
                c.gameObject.GetComponent<EnemyAiController>().AttackEnemy(10);
            }
        }
    }
}
