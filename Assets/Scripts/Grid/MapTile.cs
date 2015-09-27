using UnityEngine;

public class MapTile : MonoBehaviour {
    public bool blocked;
    public Vector2 gridPosition;
    private SpriteRenderer Renderer;

    public void Start() {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void Select(Color tintColor) {
        Renderer.color = tintColor;
    }

    public void Deselect() {
        Select(Color.white);
    }
}