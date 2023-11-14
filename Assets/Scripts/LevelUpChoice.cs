using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject resistance;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject attackSpeed;
    [SerializeField] private HUDStats   hudStats;
   
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject       levelUpPanel;
   
    private int choiceMade;


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
