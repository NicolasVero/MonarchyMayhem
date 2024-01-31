using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void Reprendre() {
       GameController.SetGameState(true);
    }
    
    public void ChangeScene(string _sceneName) {
        SceneManager.LoadScene(_sceneName);
    }

    public void Quit() {
       Application.Quit();
    }

}