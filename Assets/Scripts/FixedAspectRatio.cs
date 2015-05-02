using UnityEngine;

public class FixedAspectRatio : MonoBehaviour {
    public float aspectRatioHeight = 9f;
    public float aspectRatioWidth = 16f;

    private void Start() {
        InvokeRepeating("Resize", 0, 0.2f);
    }

    private void Resize() {
        var targetAspectRatio = aspectRatioWidth/aspectRatioHeight;
        var windowAspectRatio = Screen.width/(float) Screen.height;

        var scalingFactor = windowAspectRatio/targetAspectRatio;

        var camera = GetComponent<Camera>();

        if (scalingFactor < 1.0f) {
            var rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scalingFactor;
            rect.x = 0;
            rect.y = (1.0f - scalingFactor)/2.0f;

            camera.rect = rect;
        } else {
            var scalewidth = 1.0f/scalingFactor;

            var rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth)/2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}