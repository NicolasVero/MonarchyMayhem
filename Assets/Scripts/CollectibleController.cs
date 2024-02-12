using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour {

    private int childrenNumber;

    void Start() {
        childrenNumber = GetChildrenNumber(transform);
        int playerCollectedObjects = GetCollectibleCounter();
    }

    public int GetCollectibleCounter() {
        PickUp playerPickupScript = FindObjectOfType<PickUp>();
        return playerPickupScript.GetCollectedItemsCount();
    }

    private int GetChildrenNumber(Transform element) {
        return element.childCount;
    } 
}