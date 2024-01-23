using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    
    private int attack;
    private int knockback;
    private float range;
    private float attackSpeed;
    private float speed;

    [SerializeField] private GameObject[] models;
    private TextAsset weaponsList;

    void Start() {

        weaponsList = Resources.Load<TextAsset>("Data/WeaponsStats");

        if(weaponsList != null) {
            Debug.Log(weaponsList);
        }       
    }

    void Update() {
        
    }

    void aff() {
        Debug.Log("it works");
    }
}
