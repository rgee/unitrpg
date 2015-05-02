using UnityEngine;

public class Tray : MonoBehaviour {
    private Animator animator;
    private bool doneOnce;
    private TrayPortrait[] portraits;
    public float xThresholdPct;

    private void Start() {
        animator = GetComponent<Animator>();
        portraits = GetComponentsInChildren<TrayPortrait>();
    }

    private void Update() {
        var mouseX = Input.mousePosition.x;
        if (mouseX < 0 || mouseX > Screen.width) {
            return;
        }

        var pctToRight = mouseX/Screen.width;

        var shouldOpen = pctToRight > xThresholdPct;
        animator.SetBool("visible", shouldOpen);

        foreach (var portrait in portraits) {
            if (portrait.character.isDead) {
                print("disabling");
                portrait.gameObject.SetActive(false);
            }
        }
    }
}