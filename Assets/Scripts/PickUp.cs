using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    [SerializeField] private new AudioController audio;
    [SerializeField] private QuestController questController;
    private int collectedItemsCount = 0;

    public void InitPickUp() {
        this.questController = GameObject.FindGameObjectWithTag("QuestCanvas").GetComponent<QuestController>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("PickUp")) {   
            this.audio.PlayPickUpSFX();
            Destroy(other.gameObject);
            this.collectedItemsCount++;
            this.questController.UpdateQuestText();
        }
    }

    public int GetCollectedItemsCount() {
        return this.collectedItemsCount;
    }
}