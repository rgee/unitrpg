using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Utilities;
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
            var dimensions = GetDimensions();
            var mouseGridPosition = dimensions.GetGridPositionForWorldPosition(mousePosition);
            if (Input.GetMouseButtonDown(0)) {
                MapClicked.Dispatch(mouseGridPosition);
            } else {
                MapHovered.Dispatch(mouseGridPosition);
            }
        }

        public void MoveUnit(string id, List<Vector2> path) {
            var unitGameObject = FindUnitById(id);
            if (unitGameObject == null) {
                Debug.LogError("Cannot find unit to move by id: " + id);
                return;
            }

            var dimensoins = GetDimensions(); 
            var worldPositions = path.Skip(1).Select(pos => dimensoins.GetWorldPositionForGridPosition(pos)).ToList();
            var unitComp = unitGameObject.GetComponent<Grid.Unit>();
            StartCoroutine(unitComp.FollowPath(worldPositions));
        }

        public MapDimensions GetDimensions() {
            return new MapDimensions(Width, Height, TileSize);
        }

        private GameObject FindUnitById(string id) {
            var unitContainer = transform.FindChild("Units");

            foreach (Transform unit in unitContainer) {
                var unitComponent = unit.GetComponent<Grid.Unit>();
                if (unitComponent.Id == id) {
                    return unit.gameObject;
                }
            }

            return null;
        }

        public List<CombatantDatabase.CombatantReference> GetCombatants() {
            var unitContainer = transform.FindChild("Units").gameObject;
            var units = unitContainer.GetComponentsInChildren<Grid.Unit>();

            return units.Select(unit => {
                var character = unit.GetCharacter();
                return new CombatantDatabase.CombatantReference {
                    Id = unit.Id,
                    Position = unit.gridPosition,
                    Name = character.Name,

                    // TODO: Have dropdown for army type
                    Army = unit.friendly ? ArmyType.Friendly : ArmyType.Enemy
                };
            }).ToList();
        }

        public List<Vector2> GetObstructedPositions() {
            var obstructions = transform.FindChild("Obstructions");
            var results = new List<Vector2>();
            var dimensions = GetDimensions();

            foreach (Transform obstacle in obstructions) {
                var gridPosition = dimensions.GetGridPositionForWorldPosition(obstacle.position);
                results.Add(gridPosition);
            }
            
            return results;
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