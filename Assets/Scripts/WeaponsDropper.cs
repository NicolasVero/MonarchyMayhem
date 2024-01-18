using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponsDropper : MonoBehaviour {

    private int weaponID;

    public WeaponsDropper(int weaponID) {
        this.weaponID = weaponID;
    }

    void Awake() {

        TextAsset weapons = Resources.Load<TextAsset>("Data/Weapons");

        if(weapons != null) {
            Weapons weaponsData = JsonUtility.FromJson<Weapons>(weapons.text);
            Weapon weapon = Array.Find(weaponsData.weaponStats, e => e.id == this.weaponID);

            Debug.Log(weaponsData);
        }
    }
}