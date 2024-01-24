using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public string tier;
    public int id;
    public int attack;
    public float knockback;
    public float range;
    public float attackSpeed;
    public int weaponID;

    void Start()
    {
        this.weaponID = Weapon.getWeaponID(gameObject.name);

        // Debug.Log(this.weaponID);

        TextAsset weaponsList = Resources.Load<TextAsset>("Data/WeaponsStats");





        if(weaponsList != null) {
            WeaponsStats weaponsStats = JsonUtility.FromJson<WeaponsStats>(weaponsList.text);
            WeaponStat weapon = Array.Find(weaponsStats.weapons, e => e.id == this.weaponID);

            Debug.Log(weapon.name);
            this.name = weapon.name;
            this.tier = weapon.tier;
            this.attack = weapon.attack;
            this.range = weapon.range;
            this.attackSpeed = weapon.attackSpeed;
            this.knockback = weapon.knockback;


        }       
    }

    private static int getWeaponID(string name) {
        name = name.Replace("(Clone)", "");
        
        return Convert.ToInt32(name.Split('_')[1]);

    }
}
