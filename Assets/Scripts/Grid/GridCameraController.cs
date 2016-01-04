using UnityEngine;

public class GridCameraController : CameraController {
    private GameObject GridHighlight;
    private bool gridSelectorLocked;
    private MapGrid grid;

    void Start() {
        grid = CombatObjects.GetMap();
    }

    public override void Lock() {
        locked = true;
        gridSelectorLocked = true;
    }

    public override void Unlock() {
        locked = false;
        gridSelectorLocked = false;
    }

    public void DisbleGridSelector() {
        gridSelectorLocked = true;
    }

    public void EnableGridSelector() {
        gridSelectorLocked = false;
    }

    public new void Update() {
        base.Update();
//
//        var gridWidth = grid.tileSizeInPixels*grid.width;
//        var gridHeight = grid.tileSizeInPixels*grid.height;
//        Vector2 gridCenter = grid.transform.position + new Vector3(gridWidth/2, (gridHeight/2), 0);
//
//        var vSize = Camera.main.orthographicSize*2.0f;
//        var hSize = vSize*((float) Screen.width/Screen.height);
//
//        var halfHSize = hSize*0.5f;
//        var halfVSize = vSize*0.5f;
//
//        var gridHalfWidth = (grid.width/2.0f)*grid.tileSizeInPixels;
//        var maxX = gridCenter.x + gridHalfWidth;
//        var minX = gridCenter.x - gridHalfWidth;
//
//        var leftEdge = transform.position.x - halfHSize;
//        if (leftEdge < minX) {
//            transform.position = new Vector3(minX + halfHSize, transform.position.y, transform.position.z);
//        }
//
//        var rightEdge = transform.position.x + halfHSize;
//        if (rightEdge > maxX) {
//            transform.position = new Vector3(maxX - halfHSize, transform.position.y, transform.position.z);
//        }
//
//
//        var gridHalfHeight = (grid.height/2.0f)*grid.tileSizeInPixels;
//        var maxY = gridCenter.y + gridHalfHeight;
//        var minY = gridCenter.y - gridHalfHeight;
//
//        var topEdge = transform.position.y + halfVSize;
//        if (topEdge > maxY) {
//            transform.position = new Vector3(transform.position.x, maxY - halfVSize, transform.position.z);
//        }
//
//        var bottomEdge = transform.position.y - halfVSize;
//        if (bottomEdge < minY) {
//            transform.position = new Vector3(transform.position.x, minY + halfVSize, transform.position.z);
//        }
    }
}