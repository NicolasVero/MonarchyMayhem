using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    [SerializeField] private new AudioController audio;
    private int collectedItemsCount = 0;

    void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("PickUp")) {   
            this.audio.PlayPickUpSFX();
            Destroy(other.gameObject);
            collectedItemsCount++;
        }
    }

    public int GetCollectedItemsCount() {
        return collectedItemsCount;
    }
}