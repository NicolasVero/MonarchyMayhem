using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MovieController : MonoBehaviour {
    [SerializeField] private VideoPlayer videoPlayer; 

    void Start() {
        videoPlayer.loopPointReached += EndReached; 
        Invoke("LoadScene", (float) videoPlayer.clip.length);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) 
            LoadNextScene(); 
    }

    void EndReached(VideoPlayer video) {
        LoadNextScene(); 
    }

    void LoadNextScene() {
        SceneManager.LoadScene("GameMenu"); 
    }
}