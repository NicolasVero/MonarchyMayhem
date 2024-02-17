using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Difficulty : MonoBehaviour
{
    [SerializeField] private Canvas choice;
    [SerializeField] private StartMenuController menu;

    private string difficulty;

    void Start() {
          this.DisableChoice();
    }

//     private void Easy() {
//          difficulty = "Easy";
//          SceneManager.LoadScene("Tutorial");
//     }
   
//     private void Normal() {
//          difficulty = "Normal";
//          SceneManager.LoadScene("Tutorial");
//     }

//     private void Hard() {
//          difficulty = "Hard";
//          SceneManager.LoadScene("Tutorial");
//     }

    public void ChooseDifficulty(string choice){
          this.difficulty = choice;
          SceneManager.LoadScene("Tutorial");
    }
    
    public string Getdifficulty() {
          return this.difficulty;
    }
    public void EnableChoice() {
          this.choice.enabled = true;
    }
    public void DisableChoice() {
          this.choice.enabled = false;
    }

}
