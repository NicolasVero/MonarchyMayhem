using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour {
    [SerializeField] private Texture2D cursorTexture;

    void Start() {
        Cursor.SetCursor(this.cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
