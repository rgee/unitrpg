using UnityEngine;
using System.Collections;

public class MapTile : MonoBehaviour {

    public Vector2 gridPosition;

    public bool blocked;

    private SpriteRenderer renderer;

    public void Start() {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Select(Color tintColor) {
        renderer.color = tintColor;
    }

    public void Deselect() {
        Select(Color.white);
    }
}
