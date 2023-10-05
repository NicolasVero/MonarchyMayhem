using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    public GameObject attack;
    public GameObject health;
    public GameObject resistance;
    public int choiceMade;

    public PlayerController playerController;
    public GameObject levelUpPanel;

    public void choiceAttack() {
        playerController.updateAttack();
        resumeGame();
    }

    public void choiceHealth() {
        playerController.updateHealth();
        resumeGame();
    }

    public void choiceResistance() {
        playerController.updateResistance();
        resumeGame();
    }

    void resumeGame() {
        GameController.setGameState(true);
        GameController.setCursorVisibility(false);
        GameController.setPanelVisibility(levelUpPanel, false);   
    }

    void Update() {
        if(choiceMade >= 1) {
            attack.SetActive(false);
            health.SetActive(false);
            resistance.SetActive(false);
        }
    }
}
