using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour {

    [SerializeField] private Slider xpBar;
    [SerializeField] private PlayerController player;
 
    private void Start() {
        xpBar.maxValue = 1;
        xpBar.value = 0;
        // xpBar.maxValue = player.getMaxHealth();
        // xpBar.value = player.getHealth();
    }
 
    public void setXPBar(int xp) {
        this.xpBar.value = xp;
    }

    public void addXPBar(int xp) {
        this.xpBar.value += xp;
    } 

    public void setXPBarMax(int max) {
        this.xpBar.maxValue = max;
    }
}