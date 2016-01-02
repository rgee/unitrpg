using System.Collections;
using Grid;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace Combat {
    /**
     * Subclasses of CombatEvent will have access to a simplified
     * set of high-level actions that can be performed on the game world.
     */
    [ExecuteInEditMode]
    public abstract class CombatEvent : MonoBehaviour, ITrigger, IScriptedEvent {

        public IScriptedEvent Event {
            get { return this; }
        }

        void OnDrawGizmosSelected() {
            if (_grid != null) {
                Gizmos.color = Color.green;

                var gridSize = _grid.tileSizeInPixels;
                Gizmos.DrawWireCube(transform.position, new Vector3(gridSize, gridSize, gridSize));
            }
        }

        private GameObject _camera;
        private MapGrid _grid;

        void Start() {
            _camera = CombatObjects.GetCamera();
            _grid = CombatObjects.GetMap();
        }

        public abstract IEnumerator Play();

        protected IEnumerator PanCamera(Vector2 destination) {
            const float time = .7f;
            yield return _camera.transform.DOMove(destination, time).WaitForCompletion();
        }

        protected IEnumerator SpawnUnits(List<ScriptedEvents.SpawnableUnit> units) {
            var map = CombatObjects.GetMap();
            var spawnPointWorldSpace = map.GetWorldPosForGridPos(units[0].SpawnPoint);
            yield return StartCoroutine(PanCamera(spawnPointWorldSpace));

            var unitManager = CombatObjects.GetUnitManager();
            foreach (var unit in units) {
                var unitGameObject = Instantiate(unit.Prefab);
                var component = unitGameObject.GetComponent<Grid.Unit>();
                component.gridPosition = unit.SpawnPoint;
                component.model.GridPosition = unit.SpawnPoint;

                unitManager.SpawnUnit(unitGameObject);
            }

            yield return null;
        }

        protected IEnumerator StartDialogue() {
            yield return null;
        }
    }
}
