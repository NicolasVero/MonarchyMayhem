using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPController : MonoBehaviour
{
    PlayerController pc;
    float SavedTime = 0;
    float DelayTime = 2;

    private void OnTriggerEnter(Collider c){
        pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
        // Knight tagged as player needs to be on the "BaseCharacter" > "Body" part
        if (c.gameObject.CompareTag("Player")){
            pc.XPGain(1);
        }
    }
    private void OnTriggerStay(Collider c){
        pc = GameObject.Find(Names.MainCharacter).GetComponent<PlayerController>();
        if (c.gameObject.CompareTag("Player")){
            if( (Time.time - SavedTime) > DelayTime ) {
                SavedTime=Time.time;   
                pc.XPGain(1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
