using System.Collections;
using UnityEngine;

namespace States.Combat {
    public class SpawnReinforcements : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            var battle = CombatObjects.GetBattleManager();
            battle.StartCoroutine(SpawnUnits(animator, battle));
        }

        private IEnumerator SpawnUnits(Animator animator, BattleManager battle) {
            yield return battle.StartCoroutine(battle.SpawnReinforcements());
            animator.SetBool("reinforcements_triggered", false);
            animator.SetTrigger("reinforcements_spawned");
        }
    }
}