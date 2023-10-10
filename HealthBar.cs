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

        healthBar.maxValue = player.getMaxHealth();
        healthBar.value = player.getHealth();
    }
 
    public void setHealthBar(int hp) {
        healthBar.value = hp;
    }
}