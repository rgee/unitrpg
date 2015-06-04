using UnityEngine;

/**
 * Extend this class if you want to be notified of when
 * a scene is ready to start.
 */
public abstract class SceneEntryPoint : MonoBehaviour {
    public abstract void StartScene();

    public void OnEnable() {
        ApplicationEventBus.SceneStart.AddListener(StartScene);
    }

    public void OnDisable() {
        ApplicationEventBus.SceneStart.RemoveListener(StartScene);
    }
}
