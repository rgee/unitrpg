using UnityEngine;
using WellFired;

namespace States.Combat {
    public class PreBattleSequence : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var sequence = CombatObjects.GetIntroSequence();
            if (sequence == null) {
                animator.SetTrigger("intro_sequence_complete");
            } else {
                var seq = sequence.GetComponent<USSequencer>();
                seq.Play();
                seq.PlaybackFinished += sequencer => {
                    animator.SetTrigger("intro_sequence_complete");
                };
            }
        }
    }
}