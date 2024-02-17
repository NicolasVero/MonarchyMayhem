using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour {

    [SerializeField] private Difficulty difficultyController;
    [SerializeField] private AudioController audioController;

    void Start() {
        this.audioController.PlayMenuSFX();
    }

    public void Play() {
        this.difficultyController.EnableChoice();
    }

    public void Quit() {
        Application.Quit();
    }
}