using System.Collections;
using Combat.Interactive.Rules;
using Combat.Props;
using UnityEngine;

namespace Combat.ScriptedEvents.Chapter2 {
    [RequireComponent(typeof(TogglableTileRule))]
    public class ClinicVisit : CombatEvent {
        public Chapter2House Clinic;

        public GameObject LiatKnockDialogue;
        public GameObject LiatRecruitDialogue;

        public GameObject MaellePrefab;

        // TODO: BFS for an unoccupied spawn point near the door.
        private Vector2 _spawnPoint = new Vector2(37, 19);

        public override IEnumerator Play() {
            // 1) Start Maelle dialogues
            yield return StartCoroutine(RunDialogue(LiatKnockDialogue));
            yield return StartCoroutine(RunDialogue(LiatRecruitDialogue));

            // 2) Turn out light (save energy!)
            yield return StartCoroutine(Clinic.Disable());

            // 3) Spawn Maelle
            var battle = CombatObjects.GetBattleManager();
            var maelle = new SpawnableUnit {
                Prefab = MaellePrefab,
                SpawnPoint = _spawnPoint
            };
            yield return StartCoroutine(battle.SpawnSingleUnit(maelle));
        }
    }
}