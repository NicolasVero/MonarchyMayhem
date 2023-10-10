using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static void setGameState() {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }
    
    public static void setGameState(bool state) {
        Time.timeScale = (state) ? 1 : 0;
    }

    public static bool getGameState() {
        return Time.timeScale == 0;
    }

    public static void setCursorVisibility(bool state) {
        Cursor.visible = state;
        Cursor.lockState = (state) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static void setPanelVisibility(GameObject panel, bool state) {
        panel.SetActive(state);
    }
}
