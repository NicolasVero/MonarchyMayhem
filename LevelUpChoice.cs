using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelUpPanel : MonoBehaviour {

    public GameObject choice01;
    public GameObject choice02;
    public GameObject choice03;
    public int choiceMade;

    public void appear() {
        Debug.Log("MONTE DE NIVEAU !");
    }

    public void choiceOption1() {
        Debug.Log("Choice 1");
    }

    public void choiceOption2() {
        Debug.Log("Choice 2");
    }

    public void choiceOption3() {
        Debug.Log("Choice 3");
    }

    void Update() {
        if(choiceMade >= 1) {
            choice01.SetActive(false);
            choice02.SetActive(false);
            choice03.SetActive(false);
        }
    }
}