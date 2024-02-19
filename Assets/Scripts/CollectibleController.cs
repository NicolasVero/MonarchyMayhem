using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour {

    private int childrenNumber;
    private float floatHeight = 0.05f;
    private float floatSpeed = 1.0f; 

    void Start() {
        this.childrenNumber = GetChildrenNumber(transform);
    }

    void Update() {
        this.RotateAndFloatChildren(transform);
    }

    public int GetCollectibleCounter() {
        PickUp playerPickupScript = FindObjectOfType<PickUp>();
        return playerPickupScript.GetCollectedItemsCount();
    }

    private int GetChildrenNumber(Transform element) {
        return element.childCount;
    } 

    private void RotateAndFloatChildren(Transform parentTransform) {
        foreach(Transform child in parentTransform) {
            
            child.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
            
            float newY = Mathf.Sin(Time.time * floatSpeed) * this.floatHeight;
            child.Translate(Vector3.up * newY * Time.deltaTime, Space.World);
        }
    }
}
