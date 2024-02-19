using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsDropper : MonoBehaviour {
    
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject weaponsContainer;
    private float[] probabilities = {
        0.1f, 
        0.35f, 
        0.05f, 
        0.15f, 
        0.15f, 
        0.1f, 
        0.15f, 
        0.25f, 
        0.25f, 
        0.15f, 
        0.25f, 
        0.35f, 
        0.15f, 
        0.05f, 
        0.15f,
        0.25f, 
        0.35f, 
        0.35f, 
        0.35f, 
        0.35f,
        0.25f,
        0.35f,
        0.35f, 
        0.15f, 
        0.35f, 
        0.25f, 
        0.25f, 
        0.35f,
        0.35f, 
        0.25f, 
        0.35f, 
        0.35f, 
        0.15f, 
        0.25f
    };


    public void CreateWeapon(int weaponID, Vector3 position) {
        position.y += (position.y > 1.5) ? 0 : 1;

        weaponID = (weaponID == -1) ? GiveRandomWeaponID() : weaponID;

        var instance = Instantiate(this.models[weaponID], position, Quaternion.Euler(0f, 0f, 90f));
        instance.transform.parent = this.weaponsContainer.transform;
    }

    public void CreateWeapon(Vector3 position) {
        this.CreateWeapon(-1, position);
    }

    private int GiveRandomWeaponID() {

        while(true) {
            int number = GameController.Random(0, this.probabilities.Length - 1);

            if(GameController.RandomFloat() < this.probabilities[number]) {
                return number;
            }
        }
    }

    public int GetWeaponsListLength() {
        return this.models.Length;
    }
}
