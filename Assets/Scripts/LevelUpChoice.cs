using System;
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
    [SerializeField] private GameObject speed;
    [SerializeField] private HUDStats   hudStats;
   
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject       levelUpPanel;

    private GameObject[] banners;
    private int bannersLength = 6;
   
    // [SerializeField] private GameObject attackBanner;

    private int choiceMade;

    public void Awake() {
        
    }

    public void updateStatsDisplay() {

        this.banners = new GameObject[] {
            this.attack, 
            this.health, 
            this.resistance, 
            this.range, 
            this.attackSpeed, 
            this.speed
        };

        System.Random rnd = new System.Random();

        GameController.setGameState(false);
        GameController.setPanelVisibility(levelUpPanel, true);
        GameController.setPanelVisibility(banners[rnd.Next(this.bannersLength - 1)], false);
        GameController.setCursorVisibility(true);
    }

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

    public void choiceSpeed() {
        this.playerController.updateSpeed();
        this.resumeGame();
    }

    void resumeGame() {
        this.playerController.setCanResume(true);
        this.hudStats.updateStats();
        
        for(int i = 0; i < this.bannersLength; i++)
            GameController.setPanelVisibility(banners[i], true);

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
