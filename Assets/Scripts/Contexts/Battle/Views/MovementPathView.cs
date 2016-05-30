using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using Utils;

namespace Contexts.Battle.Views {
    public class MovementPathView : View {
        private readonly List<GameObject> _pathSprites = new List<GameObject>();
         
        public Sprite EastHead;
        public Sprite EastSegment;
        public Sprite NorthEastCorner;
        public Sprite NorthHead;
        public Sprite NorthSegment;
        public Sprite NorthWestCorner;
        public Sprite SouthEastCorner;
        public Sprite SouthHead;
        public Sprite SouthSegment;
        public Sprite SouthWestCorner;
        public Sprite WestHead;
        public Sprite WestSegment;


        public void ClearPath() {
            foreach (var sprite in _pathSprites) {
                Destroy(sprite);
            }
            _pathSprites.Clear();
        }

        private static IEnumerable<Junction> ConvertToJunctions(IList<Vector3> points) {
            if (points.Count < 2) {
                throw new ArgumentException("At least two points are required to make a path.");
            }

            var result = new List<Junction>();

            var prevIdx = 0;
            var currentIdx = 1;
            var nextIdx = 2;

            // Iterate in a sliding window of three points on the path.
            for (; currentIdx < points.Count; prevIdx++, currentIdx++, nextIdx++) {
                var junction = new Junction();

                var entrance = new Transition {Destination = points[currentIdx]};
                entrance.Direction = MathUtils.DirectionTo(points[prevIdx], entrance.Destination);
                junction.Entrance = entrance;

                // If there is no next point, we've reached the end.
                if (nextIdx < points.Count) {
                    var exit = new Transition {Destination = points[nextIdx]};
                    exit.Direction = MathUtils.DirectionTo(entrance.Destination, exit.Destination);
                    junction.Exit = exit;
                }

                result.Add(junction);
            }

            return result;
        }

        private void AddPathSegment(Vector3 position, Sprite sprite) {
            var obj = new GameObject("path_segment");
            obj.transform.position = position;

            var objRenderer = obj.AddComponent<SpriteRenderer>();
            objRenderer.sprite = sprite;
            
            obj.AddComponent<DynamicSortedObject>();

            _pathSprites.Add(obj);
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

        private Sprite GetCornerSpriteForDirections(MathUtils.CardinalDirection enterDir,
                                                    MathUtils.CardinalDirection exitDir) {
            if (enterDir == MathUtils.CardinalDirection.N) {
                if (exitDir == MathUtils.CardinalDirection.E) {
                    return NorthEastCorner;
                }
                return NorthWestCorner;
            }

            if (enterDir == MathUtils.CardinalDirection.S) {
                if (exitDir == MathUtils.CardinalDirection.E) {
                    return SouthEastCorner;
                }
                return SouthWestCorner;
            }

            if (enterDir == MathUtils.CardinalDirection.E) {
                if (exitDir == MathUtils.CardinalDirection.N) {
                    return SouthWestCorner;
                }
                return NorthWestCorner;
            }

            if (enterDir == MathUtils.CardinalDirection.W) {
                if (exitDir == MathUtils.CardinalDirection.S) {
                    return NorthEastCorner;
                }
                return SouthEastCorner;
            }

            throw new ArgumentException("uh, can't find the direction to render the path.");
        }

        public void ShowPath(List<Vector3> pathSegments) {
            ClearPath();

            var junctions = ConvertToJunctions(pathSegments);
            foreach (var jct in junctions) {
                var destination = jct.Entrance.Destination;
                var direction = jct.Entrance.Direction;

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

        private struct Junction {
            public Transition Entrance;
            public Transition? Exit;

            public bool IsCorner() {
                if (!Exit.HasValue) {
                    return false;
                }

                var entranceDirection = Entrance.Direction;
                var exitDirection = Exit.Value.Direction;
                return entranceDirection.GetOrientation() != exitDirection.GetOrientation();
            }

            public override string ToString() {
                return string.Format("Entrance: {0}, Exit: {1}", Entrance, Exit);
            }
        }

        private struct Transition {
            public Vector3 Destination;
            public MathUtils.CardinalDirection Direction;

            public override string ToString() {
                return string.Format("Destination: {0}, Direction: {1}", Destination, Direction);
            }
        }
    }
}