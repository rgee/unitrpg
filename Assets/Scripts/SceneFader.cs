using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : Singleton<SceneFader> {
    private const float fadeTimeSeconds = 0.7f;
    private Image image;
    public GameObject overlay;

    private void Awake() {
        DontDestroyOnLoad(this);
        overlay.SetActive(false);
        image = overlay.GetComponent<Image>();
    }

    public void TransitionToScene(int sceneIndex) {
        StartCoroutine(RunScenetransitionSequence(sceneIndex));
    }

    private IEnumerator RunScenetransitionSequence(int sceneIndex) {
        yield return StartCoroutine(FadeOut());
        Application.LoadLevel(sceneIndex);
        yield return StartCoroutine(FadeIn());
        ApplicationEventBus.SceneStart.Dispatch();
    }


    public void TransitionToScene(string sceneName) {
        StartCoroutine(RunScenetransitionSequence(sceneName));
    }

    private IEnumerator RunScenetransitionSequence(string sceneName) {
        yield return StartCoroutine(FadeOut());
        Application.LoadLevel(sceneName);
        yield return StartCoroutine(FadeIn());
        ApplicationEventBus.SceneStart.Dispatch();
    }

    public IEnumerator FadeOut() {
        overlay.SetActive(true);
        if (image.color.a > 0) {
            image.color = ImageColorWithAlpha(0);
        }

        yield return StartCoroutine(FadeToAlpha(1));
    }

    public IEnumerator FadeIn() {
        overlay.SetActive(true);
        if (image.color.a < 1) {
            image.color = ImageColorWithAlpha(1);
        }

        yield return StartCoroutine(FadeToAlpha(0));
    }

    private Color ImageColorWithAlpha(float alpha) {
        return new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public IEnumerator FadeToAlpha(float alpha) {
        float elapsedTime = 0;
        var startColor = image.color;
        var endColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
        while (elapsedTime < fadeTimeSeconds) {
            image.color = Color.Lerp(startColor, endColor, (elapsedTime/fadeTimeSeconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}