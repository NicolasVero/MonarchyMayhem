using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour {

    [SerializeField] private Difficulty difficultyController;

    void Start() {
        this.difficultyController = FindObjectOfType<Difficulty>();
        DontDestroyOnLoad(this.difficultyController.gameObject);
    }
}
