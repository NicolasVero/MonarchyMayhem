using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

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

			this.id = weapon.id;
			this.name = weapon.name;
			this.tier = weapon.tier;
			this.attack = weapon.attack;
			this.attackSpeed = weapon.attackSpeed;
			this.knockback = weapon.knockback;
			this.range = weapon.range;
			this.regeneration = weapon.regeneration;
			this.speed = weapon.speed;

            Transform lightTransform = transform.Find("weapon_light");
            Light lightComponent = lightTransform.GetComponent<Light>();
            lightComponent.color = Color.red; 


			Dictionary<string, string> tierColors = new Dictionary<string, string> {
				{ "S", "#FF7C00" },         
				{ "A", "#8E00FF" },
				{ "B", "#0046FF" },
				{ "C", "#77FF00" },
				{ "D", "#FFFFFF" }
			};

        string hexColor;
        tierColors.TryGetValue(tier, out hexColor);
        lightComponent.color = GetColorFromHexadecimal(hexColor);



			// if(this.tier != "S") {
			// 	lightComponent.color = GetColorFromHexadecimal("#FF7C00");
            // }

            // if(this.tier == "A") {
			// 	lightComponent.color = GetColorFromHexadecimal("#0046FF");
            // }

			// if(this.tier == "B") {
			// 	lightComponent.color = GetColorFromHexadecimal("#8E00FF");
            // }

			// if(this.tier == "C") {
			// 	lightComponent.color = GetColorFromHexadecimal("#77FF00");
            // }

			// if(this.tier == "D") {
			// 	lightComponent.color = GetColorFromHexadecimal("#A5C4C6");
            // }

            Debug.Log(lightTransform);
            Debug.Log(weapon.id);
        }       
    }

	private Color GetColorFromHexadecimal(string hexColor) {
		Color color;
        ColorUtility.TryParseHtmlString(hexColor, out color);
        return color;
	}

	private static int GetWeaponID(string name) {
		name = name.Replace("(Clone)", "");
		return Convert.ToInt32(name.Split('_')[1]);
	}
}
