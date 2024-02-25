using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class StartMenuController : MonoBehaviour {

    [SerializeField] private Difficulty difficultyController;
    [SerializeField] private AudioController audioController;

    void Start() {
        StartCoroutine(SetLanguage());
        this.audioController.PlayMenuSFX();
        GameController.SetCursorVisibility(true);
    }

    public void Play() {
        this.difficultyController.EnableChoice();
    }

    public void Quit() {
        Application.Quit();
    }

    private IEnumerator SetLanguage(){
        yield return LocalizationSettings.InitializationOperation;

        // 0 = Anglais / 1 = Français
        LocalizationSettings.SelectedLocale = (GameController.GetSystemLanguageUpper() == "FR") ? LocalizationSettings.AvailableLocales.Locales[1] : LocalizationSettings.AvailableLocales.Locales[0];
    }
}