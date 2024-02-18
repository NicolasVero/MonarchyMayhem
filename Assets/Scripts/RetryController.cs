using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryController : MonoBehaviour
{
    public void ChangeScene() {
        Destroy(GameObject.FindGameObjectWithTag(Names.MainCharacter));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
        SceneManager.LoadScene("GameMenu");
    }

    public void Quit() {
        Application.Quit();
    }
}
