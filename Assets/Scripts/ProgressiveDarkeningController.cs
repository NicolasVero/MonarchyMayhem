using UnityEngine;
using UnityEngine.UI;

public class ProgressiveDarkeningController : MonoBehaviour {

    [SerializeField] private float fadeDuration;
    private float startTime;
    private bool isFading = false;

    void Update() {
        
        if (isFading) {

            float elapsedTime = Time.realtimeSinceStartup - startTime;
            float percentage = elapsedTime / this.fadeDuration;
            percentage = Mathf.Clamp01(percentage);
            float alpha = Mathf.Lerp(0f, 1f, percentage);
            GetComponent<Image>().color = new Color(0f, 0f, 0f, alpha);

            if (alpha >= 1.0f) {
                isFading = false;
            }
        }
    }

    public void StartFading() {
        startTime = Time.realtimeSinceStartup;
        isFading = true;
    }
}
