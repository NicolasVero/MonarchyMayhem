using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    public GameObject choice01;
    public GameObject choice02;
    public GameObject choice03;
    public int choiceMade;

    public PlayerController playerController;
    public GameObject levelUpPanel;

    public void choiceOption1() {
        playerController.updateAttack();
        resumeGame();
    }

    public void choiceOption2() {
        playerController.updateHealth();
        resumeGame();
    }

    public void choiceOption3() {
        playerController.updateResistance();
        resumeGame();
    }

    static void breakGame() {

    }

    void resumeGame() {
        GameController.setGameState(true);
        GameController.setCursorVisibility(false);
        GameController.setPanelVisibility(levelUpPanel, false);   
    }

    void Update() {
        if(choiceMade >= 1) {
            choice01.SetActive(false);
            choice02.SetActive(false);
            choice03.SetActive(false);
        }
    }
}
