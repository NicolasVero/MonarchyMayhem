using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour
{

    PlayerController pc;
    float SavedTime = 0;
    float DelayTime = 2;

    private void OnTriggerEnter(Collider c){
        this.pc = GameObject.Find("Knight").GetComponent<PlayerController>();
        // Knight tagged as player needs to be on the "BaseCharacter" > "Body" part
        if (c.gameObject.CompareTag("Player")){
            this.pc.HealHB(5);
        }
    }
    private void OnTriggerStay(Collider c){
        this.pc = GameObject.Find("Knight").GetComponent<PlayerController>();
        if (c.gameObject.CompareTag("Player")){
            if( (Time.time - SavedTime) > DelayTime ) {
                SavedTime=Time.time;                            
                this.pc.HealHB(5);
            }
        } 
    }
}
