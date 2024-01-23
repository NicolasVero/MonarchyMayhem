using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsDropper : MonoBehaviour {
    
    [SerializeField] private GameObject[] models;

    public void CreateWeapon(int weaponID, Vector3 position) {
        position.y += (position.y > 1.5) ? 0 : 1;
        Instantiate(models[weaponID], position, Quaternion.Euler(0f, 0f, 90f));
    }

    public int GetWeaponsListLength() {
        return models.Length;
    }
}
