using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialScreenController : MonoBehaviour {
    [SerializeField] private Canvas tutorial;

    void Awake() {
        GameController.SetGameState(false);
    }

    public void OnMouseDown() {
        GameController.SetGameState(true);
        GameController.SetCanvasVisibility(this.tutorial, false);
    }
}