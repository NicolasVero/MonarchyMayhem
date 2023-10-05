using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour {

    public GameObject choice01;
    public GameObject choice02;
    public GameObject choice03;
    public int choiceMade;

    public GameObject levelUpPanel;

    public void choiceOption1() {
        Debug.Log("Choice 1");
        resumeGame();
    }

    public void choiceOption2() {
        Debug.Log("Choice 2");
        resumeGame();
    }

    public void choiceOption3() {
        Debug.Log("Choice 3");
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
