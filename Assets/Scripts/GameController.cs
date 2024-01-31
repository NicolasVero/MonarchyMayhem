using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private Camera playerCamera = FindObjectOfType<Camera>();
    // private Canvas pauseMenu = 


    public static void SetGameState() {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }
    
    public static void SetGameState(bool state) {
        Time.timeScale = (state) ? 1 : 0;
    }

    public static void SetGameState(float value) {
        Time.timeScale = value;
    }

    public static bool GameIsFreeze() {
        return Time.timeScale == 0;
    }

    public static void SetCursorVisibility(bool state) {
        Cursor.visible = state;
        Cursor.lockState = (state) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public static void SetPanelVisibility(GameObject panel, bool state) {
        panel.SetActive(state);
    }

    public static void SetCanvasVisibility(Canvas canvas, bool state) {
        canvas.enabled = state;
    }

    public static void DrawCircleAroundObject(Vector3 position, float range, int numRays = 36) {
        float angleIncrement = 360.0f / numRays;
        Color rayColor = Color.red;

        for (int i = 0; i < numRays; i++) {
            float angle = i * angleIncrement;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * range;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * range;

            Vector3 rayDirection = new Vector3(x, 0.0f, z);
            Debug.DrawRay(position, rayDirection, rayColor);
        }
    }

    public static void DestroyWeapon(Weapon weapon) {
        var renderer = weapon.GetComponent<Renderer>();
        var collider = weapon.GetComponent<Collider>();

        if(renderer != null) Destroy(renderer);
        if(collider != null) Destroy(collider);

        Destroy(weapon);
    }


    public static bool ShowPauseMenu(GameObject pauseMenu) {
        if(pauseMenu == null) return false;
        
        pauseMenu.SetActive(true);
        GameController.SetCursorVisibility(true);
        return true;
    }
    

    public static bool HidePauseMenu(GameObject pauseMenu) {
        if(pauseMenu == null) return false;

        pauseMenu.SetActive(false);
        GameController.SetCursorVisibility(false);
        return true;
    }
    

}
