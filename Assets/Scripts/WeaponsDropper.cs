using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponsDropper : MonoBehaviour {

    private TextAsset weaponsList;

    void Start() {

        weaponsList = Resources.Load<TextAsset>("Data/WeaponsStats");

        if(weaponsList != null) {
            Debug.Log(weaponsList);
            // WeaponsStats weaponsData = JsonUtility.FromJson<WeaponsStats>(weapons.text);
            // WeaponStat weapon = Array.Find(weaponsData.weaponStats, e => e.id == this.weaponID);

            // Debug.Log(weaponsData);
        }
    }

    public void CreateWeapon(int weaponID, Vector3 position) {
        Debug.Log("we id : " + weaponID);
        Debug.Log("ve pos : " + position);
    }
}