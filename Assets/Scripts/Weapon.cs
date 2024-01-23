using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    private int id;
    private int attack;
    private int knockback;
    private float range;
    private float attackSpeed;
    private float speed;
    private int weaponType;

    void Start()
    {
        this.weaponType = Weapon.getWeaponType(gameObject.name);
        Debug.Log("sauvé ?");
        Debug.Log(weaponType);

        TextAsset weaponsList = Resources.Load<TextAsset>("Data/WeaponsStats");
        
        if(weaponsList != null) {
            Debug.Log(weaponsList);
            WeaponsStats weaponsStats = JsonUtility.FromJson<WeaponsStats>(weaponsList.text);
            // WeaponStat weaponStat = Array.Find(weaponsStats.weapons, e => e.id)
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static int getWeaponType(string name) {
        return System.Int32.Parse(name.Split('_')[1]);
    }
}
