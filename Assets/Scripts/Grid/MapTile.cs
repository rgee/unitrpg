using UnityEngine;

public class MapTile : MonoBehaviour {
    public bool blocked;
    public Vector2 gridPosition;
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