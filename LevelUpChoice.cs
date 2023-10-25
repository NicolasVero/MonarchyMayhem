using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    public GameObject attack;
    public GameObject health;
    public GameObject resistance;
    public GameObject range;
    public GameObject attackSpeed;
    public HUDStats   hudStats;
    public int        choiceMade;

    public PlayerController playerController;
    public GameObject       levelUpPanel;

    public void choiceAttack() {
        this.playerController.updateAttack();
        this.resumeGame();
    }

    public void choiceHealth() {
        this.playerController.updateHealth();
        this.resumeGame();
    }

    public void choiceResistance() {
        this.playerController.updateResistance();
        this.resumeGame();
    }

    public void choiceAttackSpeed() {
        this.playerController.updateAttackSpeed();
        this.resumeGame();
    }

    public void choiceRange() {
        this.playerController.updateRange();
        this.resumeGame();
    }

    void resumeGame() {
        this.hudStats.updateStats();
        GameController.setGameState(true);
        GameController.setCursorVisibility(false);
        GameController.setPanelVisibility(levelUpPanel, false);   
    }

    void Update() {
        if(choiceMade >= 1) {
            this.attack.SetActive(false);
            this.health.SetActive(false);
            this.resistance.SetActive(false);
            this.range.SetActive(false);
        }
    }
}
