using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour {

	[SerializeField] private Canvas choice;
	[SerializeField] private StartMenuController menu;
	private string difficulty;

	void Start() {
		this.DisableChoice();
	}

	public void ChooseDifficulty(string choice) {
		this.difficulty = choice;
		SceneManager.LoadScene("Tutorial");
	}

	public string GetDifficulty() {
		return this.difficulty;
	}

	public void EnableChoice() {
		this.choice.enabled = true;
	}

	public void DisableChoice() {
		this.choice.enabled = false;
	}
}
