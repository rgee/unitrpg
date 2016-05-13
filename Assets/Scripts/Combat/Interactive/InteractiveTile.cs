using System.Collections;
using Combat.Interactive.Rules;
using UnityEngine;

namespace Combat.Interactive {
    [ExecuteInEditMode]
    [RequireComponent(typeof(IScriptedEvent))]
    public class InteractiveTile : MonoBehaviour {
        public Vector2 GridPosition;
        public string Id;
        public bool Repeatable;

        private ITileInteractivityRule _rule;
        private IScriptedEvent _event;
        private MapGrid _grid;

        void Awake() {
            _rule = GetComponent<ITileInteractivityRule>();
            if (_rule == null) {
                _rule = new DummyRule();
            }

            _event = GetComponent<IScriptedEvent>();
            _grid = CombatObjects.GetMap();
        }

        public bool CanBeUsed() {
            return _rule.CanBeUsed();
        }

        void Update() {
#if UNITY_EDITOR
            if (_grid != null) {
                SnapToGrid();
            }
#endif
        }

        void SnapToGrid() {
            transform.position = _grid.GetWorldPosForGridPos(GridPosition);
        }

        public IEnumerator Use(Grid.Unit unit) {
            var battle = CombatObjects.GetBattleState().Model;

            var tile = battle.GetInteractiveTileByLocation(GridPosition);
            if (tile.CanTrigger()) {
                var unitModel = unit.model;
                battle.TriggerInteractiveTile(tile, unitModel);
            }

            yield return StartCoroutine(_event.Play());
        }

        void OnDrawGizmosSelected() {
            if (_grid != null) {
                Gizmos.color = Color.green;

                var gridSize = _grid.tileSizeInPixels;
                Gizmos.DrawWireCube(transform.position, new Vector3(gridSize, gridSize, gridSize));
            }
        }
    }
}