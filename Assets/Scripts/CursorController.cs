using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CursorController : MonoBehaviour {
    [SerializeField] private Texture2D cursorTexture;
    // public Transform mCursorVisual;
    // public Vector3 mDisplacement;

    void Start() {
        Cursor.SetCursor(this.cursorTexture, Vector2.zero, CursorMode.Auto);
        // Cursor.visible = false;
        // DontDestroyOnLoad(transform.parent.gameObject);
    }

    // void Update() {
    //     mCursorVisual.position = Input.mousePosition + mDisplacement;
    // }

    // public void ToggleCursorVisibility() {
    //     Cursor.visible = !Cursor.visible;
    // }
}