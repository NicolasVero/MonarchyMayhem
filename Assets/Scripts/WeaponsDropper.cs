using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsDropper : MonoBehaviour {
    
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject weaponsContainer;
    private System.Random random = new System.Random();
    private float[] probabilities = {
        1, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f
    };


    public void CreateWeapon(int weaponID, Vector3 position) {
        position.y += (position.y > 1.5) ? 0 : 1;

        weaponID = (weaponID == -1) ? GiveRandomWeaponID() : weaponID;

        var instance = Instantiate(models[weaponID], position, Quaternion.Euler(0f, 0f, 90f));
        instance.transform.parent = this.weaponsContainer.transform;
    }

    public void CreateWeapon(Vector3 position) {
        CreateWeapon(-1, position);
    }

    private int GiveRandomWeaponID() {

        while(true) {
            int number = random.Next(0, this.probabilities.Length);

            if(random.NextDouble() < this.probabilities[number]) {
                return number;
            }
        }
    }

    public int GetWeaponsListLength() {
        return this.models.Length;
    }
}
