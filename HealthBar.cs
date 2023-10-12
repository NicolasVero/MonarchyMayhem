using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Slider healthBar;
    public PlayerController player;
 
    private void Start() {
        
        Debug.Log(healthBar);
        Debug.Log(player.getHealth());

        this.healthBar.maxValue = player.getMaxHealth();
        this.healthBar.value = player.getHealth();
    }
 
    public void setHealthBar(int hp) {
        this.healthBar.value = hp;
    }

    public void setHealthBarMax(int max) {
        this.healthBar.maxValue = max;
    }
}