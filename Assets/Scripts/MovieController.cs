using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MovieController : MonoBehaviour {
    [SerializeField] private VideoPlayer videoPlayer; 

    void Start() {
        this.videoPlayer.loopPointReached += EndReached; 
        Invoke("LoadScene", (float) this.videoPlayer.clip.length);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) 
            LoadNextScene(); 
    }

    void EndReached(VideoPlayer video) {
        LoadNextScene(); 
    }

    void LoadNextScene() {
        SceneManager.LoadScene(Names.GameMenu); 
    }
}