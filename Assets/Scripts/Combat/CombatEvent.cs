using System.Collections;
using Grid;
using UnityEngine;
using System.Collections.Generic;

namespace Combat {
    /**
     * Subclasses of CombatEvent will have access to a simplified
     * set of high-level actions that can be performed on the game world.
     */
    public abstract class CombatEvent : MonoBehaviour, ITrigger, IScriptedEvent {
        public abstract HashSet<Vector2> Locations { get; }

        public IScriptedEvent Event {
            get { return this; }
        }

        private GameObject _camera;

        void Start() {
            _camera = CombatObjects.GetCamera();
        }

        public abstract IEnumerator Play();

        protected IEnumerator PanCamera(Vector2 destination) {
            var time =  .7f;
            iTween.MoveTo(_camera, iTween.Hash(
                "x", destination.x,
                "y", destination.y,
                "time", time
            ));

            yield return new WaitForSeconds(time);
        }

        protected IEnumerator SpawnUnits(List<ScriptedEvents.SpawnableUnit> units) {
            var map = CombatObjects.GetMap();
            var spawnPointWorldSpace = map.GetWorldPosForGridPos(units[0].SpawnPoint);
            yield return StartCoroutine(PanCamera(spawnPointWorldSpace));

            var unitManager = CombatObjects.GetUnitManager();
            foreach (var unit in units) {
                var gameObject = Instantiate(unit.Prefab);
                var component = gameObject.GetComponent<Grid.Unit>();
                component.gridPosition = unit.SpawnPoint;
                component.model.GridPosition = unit.SpawnPoint;

                unitManager.SpawnUnit(gameObject);
            }

            yield return null;
        }

        protected IEnumerator StartDialogue() {
            yield return null;
        }
    }
}
