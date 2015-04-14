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
            Destroy(sprite);
        }
        PathSprites.Clear();
    }

    private struct Junction {
        public Transition Entrance;
        public Transition? Exit;

        public bool IsCorner() {
            if (!Exit.HasValue) { 
                return false;
            }

            MathUtils.CardinalDirection entranceDirection = Entrance.Direction;
            MathUtils.CardinalDirection exitDirection = Exit.Value.Direction;
            return entranceDirection.GetOrientation() != exitDirection.GetOrientation();
        }
    }

    private struct Transition {
        public Vector3 Destination;
        public MathUtils.CardinalDirection Direction;
    }

    private List<Junction> ConvertToJunctions(List<Vector3> points) {
        if (points.Count < 2) {
            throw new ArgumentException("At least two points are required to make a path.");
        }

        List<Junction> result = new List<Junction>();

        int prevIdx = 0;
        int currentIdx = 1;
        int nextIdx = 2;

        // Iterate in a sliding window of three points on the path.
        for (; currentIdx < points.Count; prevIdx++, currentIdx++, nextIdx++) {
            Junction junction = new Junction();

            Transition entrance = new Transition();
            entrance.Destination = points[currentIdx];
            entrance.Direction = MathUtils.DirectionTo(points[prevIdx], entrance.Destination);
            junction.Entrance = entrance;

            // If there is no next point, we've reached the end.
            if (nextIdx < points.Count) {
                Transition exit = new Transition();
                exit.Destination = points[nextIdx];
                exit.Direction = MathUtils.DirectionTo(entrance.Destination, exit.Destination);
                junction.Exit = exit;
            }

            result.Add(junction);
        }

        return result;
    }

    private void AddPathSegment(Vector3 position, Sprite sprite) {
        GameObject obj = new GameObject("path_segment");
        obj.transform.position = position;
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 16;

        PathSprites.Add(obj);
    }

    private Sprite GetSpriteForDirection(MathUtils.CardinalDirection dir) {
        switch (dir) {
            case MathUtils.CardinalDirection.E:
                return EastSegment;
            case MathUtils.CardinalDirection.W:
                return WestSegment;
            case MathUtils.CardinalDirection.S:
                return SouthSegment;
            case MathUtils.CardinalDirection.N:
                return NorthSegment;
            default:
                throw new ArgumentException("uh, can't find the direction to render the path.");
        }
    }

    private Sprite GetHeadSpriteForDirection(MathUtils.CardinalDirection dir) {
        switch (dir) {
            case MathUtils.CardinalDirection.E:
                return EastHead;
            case MathUtils.CardinalDirection.W:
                return WestHead;
            case MathUtils.CardinalDirection.S:
                return SouthHead;
            case MathUtils.CardinalDirection.N:
                return NorthHead;
            default:
                throw new ArgumentException("uh, can't find the direction to render the path.");
        }
    }

    private Sprite GetCornerSpriteForDirections(MathUtils.CardinalDirection enterDir, MathUtils.CardinalDirection exitDir) {
        if (enterDir == MathUtils.CardinalDirection.N) {
            if (exitDir == MathUtils.CardinalDirection.E) {
                return NorthEastCorner;
            } else {
                return NorthWestCorner;
            }
        }

        if (enterDir == MathUtils.CardinalDirection.S) {
            if (exitDir == MathUtils.CardinalDirection.E) {
                return SouthEastCorner;
            } else {
                return SouthWestCorner;
            }
        }

        if (enterDir == MathUtils.CardinalDirection.E) {
            if (exitDir == MathUtils.CardinalDirection.N) {
                return SouthWestCorner;
            } else {
                return NorthWestCorner;
            }
        }

        if (enterDir == MathUtils.CardinalDirection.W) {
            if (exitDir == MathUtils.CardinalDirection.S) {
                return NorthEastCorner;
            } else {
                return SouthEastCorner;
            }
        }

        throw new ArgumentException("uh, can't find the direction to render the path.");
    }

    public void ShowPath(List<Vector3> pathSegments) {
        ClearPath();

        List<Junction> junctions = ConvertToJunctions(pathSegments);
        foreach (Junction jct in junctions) {
            Vector3 destination = jct.Entrance.Destination;
            MathUtils.CardinalDirection direction = jct.Entrance.Direction;

            if (jct.IsCorner()) {
                AddPathSegment(destination, GetCornerSpriteForDirections(direction, jct.Exit.Value.Direction));
            } else {
                if (jct.Exit.HasValue) {
                    AddPathSegment(destination, GetSpriteForDirection(jct.Entrance.Direction));
                } else {
                    AddPathSegment(destination, GetHeadSpriteForDirection(jct.Entrance.Direction));
                }
            }
        }
    }
}
