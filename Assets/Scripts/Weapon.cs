using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponID;
    public string name;
    public string tier;
    public int id;
    public int attack;
    public float attackSpeed;
    public float knockback;
    public float range;
    public int regeneration;
    public float speed;

    void Start() {

        this.weaponID = Weapon.GetWeaponID(gameObject.name);
        TextAsset weaponsList = Resources.Load<TextAsset>("Data/WeaponsStats");

        if(weaponsList != null) {
            WeaponsStats weaponsStats = JsonUtility.FromJson<WeaponsStats>(weaponsList.text);
            WeaponStat weapon = Array.Find(weaponsStats.weapons, e => e.id == this.weaponID);

            this.name = weapon.name;
            this.tier = weapon.tier;
            this.attack = weapon.attack;
            this.attackSpeed = weapon.attackSpeed;
            this.knockback = weapon.knockback;
            this.range = weapon.range;
            this.regeneration = weapon.regeneration;
            this.speed = weapon.speed;

            Debug.Log(weapon.id);
        }       
    }

    private static int GetWeaponID(string name) {
        name = name.Replace("(Clone)", "");
        return Convert.ToInt32(name.Split('_')[1]);
    }
}
