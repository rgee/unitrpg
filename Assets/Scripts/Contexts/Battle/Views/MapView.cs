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
        public int Width;
        public int Height;

        private GameObject _units;

        protected override void Awake() {
            base.Awake();
            // Get the map manager component           
            _units = transform.FindChild("Units").gameObject;
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var gridPosition = GetGridPositionForWorldPosition(clickPosition);
                if (gridPosition.HasValue) {
                    MapClicked.Dispatch(gridPosition.Value);
                }
            }
        }

        private Vector2? GetGridPositionForWorldPosition(Vector3 worldPosition) {
            // use the map manager component
            return null;
        }

        public List<CombatantDatabase.CombatantReference> GetCombatants() {
            var units = _units.GetComponentsInChildren<Grid.Unit>();

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
    }
}