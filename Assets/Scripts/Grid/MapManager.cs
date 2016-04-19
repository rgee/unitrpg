using System;
using UnityEngine;

namespace Grid {
    public class MapManager : MonoBehaviour {
        public int Width;
        public int Height;
        public int GridSize;

        void Awake() {
            
        }

        public Vector3 GetSnappedGridPosition(Vector3 position) {
            var maxX = transform.position.x + (Width*GridSize);
            var maxY = transform.position.y + (Height*GridSize);
            var minX = transform.position.x;
            var minY = transform.position.y;
            var offset = Mathf.FloorToInt(GridSize/2.0f);

            var gridX = Math.Floor(MathUtils.MapRange(minX, maxX, 0, Width, position.x + offset));
            var gridY = Math.Floor(MathUtils.MapRange(minY, maxY, 0, Height, position.y + offset));

            return new Vector3((float)gridX*GridSize+transform.position.x, (float)gridY*GridSize+transform.position.y, position.z);

        }

        void OnDrawGizmos() {
            // Draw a green outline around the map.
            Gizmos.color = Color.green;
            var totalWidth = Width*GridSize;
            var totalHeight = Height*GridSize;

            var offset = new Vector3(-GridSize / 2.0f, -GridSize / 2.0f);

            var tlCorner = transform.position + offset +new Vector3(0, totalHeight);
            var trCorner = transform.position + offset + new Vector3(totalWidth, totalHeight);
            var blCorner = transform.position + offset;
            var brCorner = transform.position + offset + new Vector3(totalWidth, 0);

            Gizmos.DrawLine(tlCorner, trCorner);
            Gizmos.DrawLine(trCorner, brCorner);
            Gizmos.DrawLine(brCorner, blCorner);
            Gizmos.DrawLine(blCorner, tlCorner);
        }
    }
}