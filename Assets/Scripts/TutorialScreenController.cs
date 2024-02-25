using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialScreenController : MonoBehaviour {

    void Start() {
        this.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Interface/Backgrounds/keyboard_"+GameController.GetSystemLanguageLower());
    }

    void Awake() {
        GameController.SetGameState(false);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            this.DeactivateCanvas();
    }

    public void DeactivateCanvas() {
        Destroy(this.gameObject);
        GameController.SetGameState(true);
    }
}