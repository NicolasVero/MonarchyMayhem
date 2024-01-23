using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponsDropper : MonoBehaviour {

    [SerializeField] private GameObject[] models;
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

        position.y += (position.y > 1.5) ? 0 : 1;

        var a = Instantiate(models[weaponID], position, Quaternion.Euler(0f, 0f, 90f));
        Debug.Log(a);
    }

    public int GetWeaponsListLength() {
        return models.Length;
    }
}