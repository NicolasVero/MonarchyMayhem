using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour {

    [SerializeField] private Difficulty difficultyController;

    void Start() {
        difficultyController = FindObjectOfType<Difficulty>();
        DontDestroyOnLoad(difficultyController.gameObject);
    }
}
