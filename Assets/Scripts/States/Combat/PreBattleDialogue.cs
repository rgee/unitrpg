using UnityEngine;

namespace States.Combat {
    public class PreBattleDialogue : StateMachineBehaviour {
        private Animator _animator;
        private Dialogue _dialogue;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _animator = animator;

            var dialogueObject = GameObject.Find("Dialogues/Intro");
            if (dialogueObject == null) {
                End();
            } else {
                _dialogue = dialogueObject.GetComponent<Dialogue>();
                _dialogue.OnComplete += End;
                _dialogue.Begin();
            }

        }

        private void End() {
            _animator.SetTrigger("pre_battle_dialogue_complete");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (_dialogue) {
                _dialogue.OnComplete -= End;
            }
        }
    }
}