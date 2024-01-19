using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponsDropper : ScriptableObject {

    private int weaponID;


    public void Initialize(int weaponID) {

        this.weaponID = weaponID;

        TextAsset weapons = Resources.Load<TextAsset>("Data/WeaponsStats");


        if(weapons != null) {
            Debug.Log(weapons);
            WeaponsStats weaponsData = JsonUtility.FromJson<WeaponsStats>(weapons.text);
            WeaponStat weapon = Array.Find(weaponsData.weaponStats, e => e.id == this.weaponID);

            Debug.Log(weaponsData);
        }
    }

    public void aff() {
        Debug.Log("works");
    }
}