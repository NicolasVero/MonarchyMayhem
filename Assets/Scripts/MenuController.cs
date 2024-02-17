using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField] private PlayerController playerController;

    public void Resume() {
        GameController.SetGameState(true);
        this.playerController.SetInPause(false);
        this.playerController.ManagePauseMenu();
    }
    
    public void ChangeScene() {
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit() {
        Application.Quit();
    }
}