using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {


    [SerializeField] private PlayerController playerController;

    public void Reprendre() {
        GameController.SetGameState(true);
        this.playerController.SetInPause(false);
        this.playerController.ManagePauseMenu();
    }
    
    public void ChangeScene(string _sceneName) {
        SceneManager.LoadScene(_sceneName);
    }

    public void Quit() {
        Application.Quit();
    }

}