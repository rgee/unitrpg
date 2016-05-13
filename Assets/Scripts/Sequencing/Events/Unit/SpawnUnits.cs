using System.Collections.Generic;
using Combat.ScriptedEvents;
using UnityEngine;
using WellFired;

namespace Sequencing.Events.Unit {
    [USequencerFriendlyName("Spawn Units")]
    [USequencerEvent("Unit/Spawn")]        
    [USequencerEventHideDuration]
    [ExecuteInEditMode]
    public class SpawnUnits : USEventBase {
        public List<SpawnableUnit> Units;

        void Awake() {
            Duration = 0.7f;
        }

        public override void FireEvent() {
            var unitManager = CombatObjects.GetUnitManager();
            foreach (var unit in Units) {
                var unitGameObject = Instantiate(unit.Prefab);
                var component = unitGameObject.GetComponent<Grid.Unit>();
                component.gridPosition = unit.SpawnPoint;
                component.model.GridPosition = unit.SpawnPoint;

                unitManager.SpawnUnit(unitGameObject);
            }
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}