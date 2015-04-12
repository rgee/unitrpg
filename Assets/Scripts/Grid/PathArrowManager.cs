using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PathArrowManager : Singleton<PathArrowManager> {
    public Sprite NorthSegment;
    public Sprite SouthSegment;
    public Sprite EastSegment;
    public Sprite WestSegment;

    public Sprite NorthHead;
    public Sprite SouthHead;
    public Sprite EastHead;
    public Sprite WestHead;

    public Sprite NorthWestCorner;
    public Sprite SouthWestCorner;
    public Sprite NorthEastCorner;
    public Sprite SouthEastCorner;


    private List<GameObject> PathSprites = new List<GameObject>();

    public void ClearPath() {
        foreach (GameObject sprite in PathSprites) {
            Debug.Log("Destroying path");
            Destroy(sprite);
        }
        PathSprites.Clear();
    }

    public void ShowPath(List<Vector3> pathSegments) {
        ClearPath();

        Vector3 prevSegment = pathSegments[0];
        foreach (Vector3 segment in pathSegments.GetRange(1, pathSegments.Count - 1)) {
            MathUtils.CardinalDirection dir = MathUtils.DirectionTo(prevSegment, segment);
            Sprite sprite;
            switch (dir) {
                case MathUtils.CardinalDirection.E:
                    sprite = EastSegment;
                    break;
                case MathUtils.CardinalDirection.W:
                    sprite = WestSegment;
                    break;
                case MathUtils.CardinalDirection.S:
                    sprite = SouthSegment;
                    break;
                case MathUtils.CardinalDirection.N:
                    sprite = NorthSegment;
                    break;
                default:
                    throw new ArgumentException("uh, can't find the direction to render the path.");
            }

            GameObject obj = new GameObject("path_segment");
            obj.transform.position = segment;
            SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 16;

            PathSprites.Add(obj);

            prevSegment = segment;
        }

        GameObject arrowHead = PathSprites.Last();
        GameObject headConnector = PathSprites[PathSprites.Count - 2];

        MathUtils.CardinalDirection dirToEnd = MathUtils.DirectionTo(headConnector.transform.position, arrowHead.transform.position);
        Sprite arrowHeadSprite;
        switch (dirToEnd) {
            case MathUtils.CardinalDirection.E:
                arrowHeadSprite = EastHead;
                break;
            case MathUtils.CardinalDirection.W:
                arrowHeadSprite = WestHead;
                break;
            case MathUtils.CardinalDirection.S:
                arrowHeadSprite = SouthHead;
                break;
            case MathUtils.CardinalDirection.N:
                arrowHeadSprite = NorthHead;
                break;
            default:
                throw new ArgumentException("uh, can't find the direction to render the path.");
        }
        arrowHead.GetComponent<SpriteRenderer>().sprite = arrowHeadSprite;

        Debug.Log("Added " + PathSprites.Count + " segment sprites");
    }
}
