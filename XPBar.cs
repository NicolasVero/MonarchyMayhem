using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour {

    public Slider xpBar;
    public PlayerController player;
 
    private void Start() {
        
        Debug.Log(xpBar);
        Debug.Log(player.getHealth());

        xpBar.maxValue = 1;
        xpBar.value = 0;
        // xpBar.maxValue = player.getMaxHealth();
        // xpBar.value = player.getHealth();
    }
 
    public void setXPBar(int xp) {
        xpBar.value = xp;
    }

    public void addXPBar(int xp) {
        xpBar.value += xp;
    } 

    public void setXPBarMax(int max) {
        xpBar.maxValue = max;
    }
}