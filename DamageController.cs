using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{

    PlayerController pc;
    float SavedTime = 0;
    float DelayTime = 2;

    private void OnTriggerEnter(Collider c){
        
        Debug.Log(pc.getResistance());
        this.pc = GameObject.Find("Knight").GetComponent<PlayerController>();
        // Knight tagged as player needs to be on the "BaseCharacter" > "Body" part
        if (c.gameObject.CompareTag("Player")){
            this.pc.TakeDamage(getRealDamage(getDamage(), this.pc.getResistance()));
        }
    }
    private void OnTriggerStay(Collider c){
        this.pc = GameObject.Find("Knight").GetComponent<PlayerController>();
        if (c.gameObject.CompareTag("Player")){
            if( (Time.time - SavedTime) > DelayTime ) {
                SavedTime=Time.time;   
                this.pc.TakeDamage(getRealDamage(getDamage(), pc.getResistance()));
            }
        }
    }

    private int getDamage() {
        return 5;
    }

    private int getRealDamage(int damage, int resistance) {
        Debug.Log("REAL DAMAGE : " + (int)(damage * (1.0 - resistance / 100.0)));
        return (int)(damage * (1.0 - resistance / 100.0));
    }
    
}
