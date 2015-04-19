using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CombatCursor : Singleton<CombatCursor> {

    private bool interactive;
    public bool Interactive {
        get { return interactive; }
        set {
            Animator.SetBool("Interactive", value);
        }
    }

    private RectTransform Transform;
    private Animator Animator;

    public void Start() {
        Transform = GetComponent<RectTransform>();
        Animator = GetComponent<Animator>();
    }

    public void Update() {
        Vector2 size = Transform.sizeDelta;
        size.Scale(new Vector2(.5f * Transform.localScale.x, .5f * transform.localScale.y));

        Transform.anchoredPosition3D = Input.mousePosition - new Vector3(size.x, size.y);
    }

    public void OnDisable() {
        Cursor.visible = true;
    }

    public void OnEnable() {
        Cursor.visible = false;
    }
}
