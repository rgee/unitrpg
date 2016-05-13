using System;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    // Just a little value object to get around the single parameter, 
    // command-bound signal limitation.
    public class MapDimensions {
        public readonly int Width;
        public readonly int Height;
        public readonly int TileSize;

        public MapDimensions(int width, int height, int tileSize) {
            Width = width;
            Height = height;
            TileSize = tileSize;
        }

        public Vector2 GetGridPositionForWorldPosition(Vector3 worldPosition) {
            var widthExtent = Width * TileSize;
            var heightExtent = Height * TileSize;
            return new Vector2(
                (float)Math.Floor(MathUtils.MapRange(0, widthExtent, 0, Width, worldPosition.x + Mathf.FloorToInt(TileSize / 2f))),
                (float)Math.Floor(MathUtils.MapRange(0, heightExtent, 0, Height, worldPosition.y + Mathf.FloorToInt(TileSize / 2f)))
            );
        }

        public Vector3 GetWorldPositionForGridPosition(Vector2 gridPosition) {

            var widthExtent = Width * TileSize;
            var heightExtent = Height * TileSize;

            // Map the input values for the x and y axis in grid space to world space.
            // Be sure to output the center of the tile in world space by adding
            // 1/2 the tile height and width!
            var result = new Vector3(
                MathUtils.MapRange(0, Width, 0, widthExtent, gridPosition.x),
                MathUtils.MapRange(0, Height, 0, heightExtent, gridPosition.y),
                0
            );

            return result;
        }

    }
}