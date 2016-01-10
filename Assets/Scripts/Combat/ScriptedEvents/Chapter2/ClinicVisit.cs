using System.Collections;
using Combat.Interactive.Rules;
using Combat.Props;
using UnityEngine;

namespace Combat.ScriptedEvents.Chapter2 {
    [RequireComponent(typeof(TogglableTileRule))]
    public class ClinicVisit : MonoBehaviour, IScriptedEvent {
        public Chapter2House Clinic;

        public GameObject MaellePrefab;

        // TODO: BFS for an unoccupied spawn point near the door.
        private Vector2 _spawnPoint = new Vector2(37, 19);

        public IEnumerator Play() {
            // 1) Start Maelle dialogue

            // 2) Spawn Maelle
            var battle = CombatObjects.GetBattleManager();
            var maelle = new SpawnableUnit {
                Prefab = MaellePrefab,
                SpawnPoint = _spawnPoint
            };
            yield return StartCoroutine(battle.SpawnSingleUnit(maelle));

            // 3) Turn out light (save energy!)
            yield return StartCoroutine(Clinic.Disable());
        }
    }
}