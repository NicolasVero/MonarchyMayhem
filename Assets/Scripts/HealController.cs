using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour {
    private PlayerController pc;
    private float SavedTime = 0;
    private float DelayTime = 2;

    private void OnTriggerEnter(Collider c) {
        this.pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();

        if (c.gameObject.CompareTag("Player")){
            this.pc.HealHB(5);
        }
    }
    
    private void OnTriggerStay(Collider c) {
        this.pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
        if (c.gameObject.CompareTag("Player")){
            if( (Time.time - SavedTime) > DelayTime ) {
                SavedTime=Time.time;                            
                this.pc.HealHB(5);
            }
        } 
    }
}
