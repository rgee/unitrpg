using System.Collections;
using Models.Combat;
using UnityEngine;

namespace Combat.Interactive {
    [RequireComponent(typeof(ITileInteractivityRule))]
    [RequireComponent(typeof(IScriptedEvent))]
    public class InteractiveTile : MonoBehaviour {
        public Vector2 GridPosition;
        public string Id;

        private ITileInteractivityRule _rule;
        private IScriptedEvent _event;

        void Awake() {
            _rule = GetComponent<ITileInteractivityRule>();
            _event = GetComponent<IScriptedEvent>();
        }

        bool CanBeUsed() {
            return _rule.CanBeUsed();
        }

        IEnumerator Use(Grid.Unit unit) {
            var battle = CombatObjects.GetBattleState().Model;

            var tile = battle.GetInteractiveTileByLocation(GridPosition);
            if (tile.CanTrigger()) {
                var unitModel = unit.model;
                battle.TriggerInteractiveTile(tile, unitModel);
            }

            yield return _event.Play();
        }
    }
}