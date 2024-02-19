using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryController : MonoBehaviour
{
    public void ChangeScene() {
        Destroy(GameObject.FindGameObjectWithTag(Names.MainCharacter));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        Destroy(GameObject.FindGameObjectWithTag("Difficulty"));
        SceneManager.LoadScene("GameMenu");
    }

    public void Quit() {
        Application.Quit();
    }
}