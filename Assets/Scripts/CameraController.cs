using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Material blackAndWhiteMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, this.blackAndWhiteMaterial);
    }

    public void EnableBlackAndWhiteEffect() {
        enabled = true;
    }

    public void DisableBlackAndWhiteEffect() {
        enabled = false;
    }
}
