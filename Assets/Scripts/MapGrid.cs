using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapGrid : MonoBehaviour {

    public string tileTag = "Tile";
    public float tileSizeInPixels = 32f;
    public int width;
    public int test;
    public int height;

    public GameObject defaultTile;

    private Dictionary<Vector2, MapTile> tilesByPosition = new Dictionary<Vector2, MapTile>();

    public void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(width * tileSizeInPixels, height * tileSizeInPixels, 0));
    }

    public void Awake() {
        foreach (Transform child in transform) {
            MapTile tile = child.GetComponent<MapTile>();
            tilesByPosition.Add(tile.gridPosition, tile);
        }
    }

    public Vector2? GetMouseGridPosition() {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        int worldStartX = -((width / 2) * (int)tileSizeInPixels);
        int worldEndX = -worldStartX;

        int worldX = ((int)mousePos.x - worldStartX) * (width) / (worldEndX - worldStartX);


        int worldStartY = -((height / 2) * (int)tileSizeInPixels);
        int worldEndY = -worldStartY;

        int worldY = ((int)mousePos.y - worldStartY) * (height) / (worldEndY - worldStartY);


        // Reject if the grid position is out of bounds.
        Vector2 gridPos = new Vector2(worldX, worldY);
        if (!IsInGrid(gridPos)) {
                return null;
        }

        return gridPos;
    }

    private bool IsInGrid(Vector2 gridPos) {
        return gridPos.x >= 0 && gridPos.x < width &&
               gridPos.y < height && gridPos.y >= 0;
    }

    public void ResetTiles() {

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tileTag)) {
            DestroyImmediate(obj);
            tilesByPosition = new Dictionary<Vector2, MapTile>();
        }

        Vector2 tileOrigin = new Vector2(
            -((width) / 2) * tileSizeInPixels ,
            -((height) / 2) * tileSizeInPixels
        );

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                GameObject tile = Instantiate(defaultTile) as GameObject;

                Vector2 gridPos = new Vector2(col, row);
                MapTile tileComponent = tile.GetComponent<MapTile>();
                tileComponent.gridPosition = gridPos;
                tilesByPosition.Add(gridPos, tileComponent);
                

                tile.transform.parent = transform;
                tile.tag = tileTag;
                tile.name = System.String.Format("({0}, {1})", col, row);

                float xOffset = col * tileSizeInPixels;
                float yOffset = row * tileSizeInPixels;
                tile.transform.position = new Vector3(tileOrigin.x + xOffset , tileOrigin.y + yOffset + (tileSizeInPixels/2), 0);
            }
        }
    }

    public GameObject GetTileAt(Vector2 position) {
        return tilesByPosition[position].gameObject;
    }
}
