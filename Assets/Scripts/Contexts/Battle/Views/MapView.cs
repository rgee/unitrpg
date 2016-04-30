using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapView : View {
        public Signal<Vector2> MapClicked = new Signal<Vector2>();
        public Signal<Vector2> MapHovered = new Signal<Vector2>(); 
        public int Width;
        public int TileSize;
        public int Height;

        void Update() {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouseGridPosition = GetGridPositionForWorldPosition(mousePosition);
            if (Input.GetMouseButtonDown(0)) {
                MapClicked.Dispatch(mouseGridPosition);
            } else {
                MapHovered.Dispatch(mouseGridPosition);
            }
        }

        private Vector2 GetGridPositionForWorldPosition(Vector3 worldPosition) {
            var widthExtent = Width*TileSize;
            var heightExtent = Height*TileSize;
            return new Vector2(
                (float) Math.Floor(MathUtils.MapRange(0, widthExtent, 0, Width, worldPosition.x + Mathf.FloorToInt(TileSize/2f))),
                (float) Math.Floor(MathUtils.MapRange(0, heightExtent, 0, Height, worldPosition.y + Mathf.FloorToInt(TileSize/2f)))
            );
        }

        public Vector3 GetWorldPositionForGridPosition(Vector2 gridPosition) {
            
            var widthExtent = Width*TileSize;
            var heightExtent = Height*TileSize;

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

        public List<CombatantDatabase.CombatantReference> GetCombatants() {
            var unitContainer = transform.FindChild("Units").gameObject;
            var units = unitContainer.GetComponentsInChildren<Grid.Unit>();

            return units.Select(unit => {
                var character = unit.GetCharacter();
                return new CombatantDatabase.CombatantReference {
                    Position = unit.gridPosition,
                    Name = character.Name,

                    // TODO: Have dropdown for army type
                    Army = unit.friendly ? ArmyType.Friendly : ArmyType.Enemy
                };
            }).ToList();
        } 

        void OnDrawGizmos() {
            // Draw a green outline around the map.
            Gizmos.color = Color.green;
            var totalWidth = Width*TileSize;
            var totalHeight = Height*TileSize;

            var offset = new Vector3(-TileSize / 2.0f, -TileSize / 2.0f);

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