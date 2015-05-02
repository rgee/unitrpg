using UnityEngine;

public class CombatCursor : Singleton<CombatCursor> {
    private Animator Animator;
    private bool interactive;
    private RectTransform Transform;

    public bool Interactive {
        get { return interactive; }
        set { Animator.SetBool("Interactive", value); }
    }

    public void Start() {
        Transform = GetComponent<RectTransform>();
        Animator = GetComponent<Animator>();
    }

    public void Update() {
        var size = Transform.sizeDelta;
        size.Scale(new Vector2(.5f*Transform.localScale.x, .5f*transform.localScale.y));

        Transform.anchoredPosition3D = Input.mousePosition - new Vector3(size.x, size.y);
    }

    public void OnDisable() {
        Cursor.visible = true;
    }

    public void OnEnable() {
        Cursor.visible = false;
    }
}