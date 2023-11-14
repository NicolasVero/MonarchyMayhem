using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] private Slider healthBar;
    [SerializeField] private PlayerController player;
 
    private void Start() {
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