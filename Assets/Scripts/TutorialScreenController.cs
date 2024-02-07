using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialScreenController : MonoBehaviour {
    [SerializeField] private Canvas tutorial;
    [SerializeField] private Canvas[] maskedCanvas;
    [SerializeField] private PlayerController player;

    void Awake() {
        ToggleCanvas(false);
        GameController.SetGameState(false);
    }

    public void OnMouseDown() {
        GameController.SetGameState(true);
        GameController.SetCanvasVisibility(this.tutorial, false);
        ToggleCanvas(true);
    }

    private void ToggleCanvas(bool state) {
        foreach(var canvas in maskedCanvas) {
            GameController.SetCanvasVisibility(canvas, state);
        }
    }
}