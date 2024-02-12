using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    [SerializeField] private new AudioController audio;
    [SerializeField] private QuestController questController;
    private int collectedItemsCount = 0;

    void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("PickUp")) {   
            this.audio.PlayPickUpSFX();
            Destroy(other.gameObject);
            collectedItemsCount++;
            Debug.Log("item ramassÃ© : " + collectedItemsCount);
            questController.UpdateQuestText();
        }
    }

    public int GetCollectedItemsCount() {
        return collectedItemsCount;
    }
}