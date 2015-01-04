using UnityEngine;
using System.Collections;

public class MapGrid : MonoBehaviour {

    public string tileTag = "Tile";
    public float tileSizeInPixels = 32f;
    public int width;
    public int test;
    public int height;

    public GameObject defaultTile;

    public void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(width * tileSizeInPixels, height * tileSizeInPixels, 0));
    }

    public void ResetTiles() {

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tileTag)) {
            DestroyImmediate(obj);
        }

        Vector2 tileOrigin = new Vector2(
            -((width) / 2) * tileSizeInPixels,
            -((height) / 2) * tileSizeInPixels
        );

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                GameObject tile = Instantiate(defaultTile) as GameObject;
                tile.transform.parent = transform;
                tile.tag = tileTag;
                tile.name = System.String.Format("({0}, {1})", col, row);

                float xOffset = col * tileSizeInPixels;
                float yOffset = row * tileSizeInPixels;
                tile.transform.position = new Vector3(tileOrigin.x + xOffset , tileOrigin.y + yOffset + (tileSizeInPixels/2), 0);
            }
        }
    }
}
