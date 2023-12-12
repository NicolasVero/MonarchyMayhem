using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {
    
    PlayerController pc;
    float SavedTime = 0;
    float DelayTime = 2;

    private void OnTriggerEnter(Collider c) {
        
        this.pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();

        if (c.gameObject.CompareTag("Player")){
            this.pc.TakeDamage(this.getRealDamage(this.getDamage(), this.pc.getResistance()));
        }
    }

    private void OnTriggerStay(Collider c) {
        this.pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
        if (c.gameObject.CompareTag("Player")){
            if( (Time.time - SavedTime) > DelayTime ) {
                SavedTime = Time.time;   
                this.pc.TakeDamage(this.getRealDamage(this.getDamage(), this.pc.getResistance()));
            }
        }
    }

    private int getDamage() {
        return 5;
    }

    private int getRealDamage(int damage, int resistance) {
        return (int)(damage * (1.0 - resistance / 100.0));
    }
    
}
