using System;
using System.Collections;
using System.IO;
using Assets.Contexts.OverlayDialogue.Signals.Public;
using Contexts.Battle.Models;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2 {
    public class BindBattleEventsCommand : Command {
        [Inject]
        public StartDialogueSignal StartDialogueSignal { get; set; }

        [Inject]
        public BattleEventRegistry EventRegistry { get; set; }

        [Inject]
        public DialogueCompleteSignal DialogueCompleteSignal { get; set; }

        [Inject]
        public BattleViewState BattleModel { get; set; }

        public override void Execute() {
            EventRegistry.RegisterHandler("inspect_inn", ShowVisit());
        }

        private IEnumerator ShowVisit() {
            var dialogueComplete = false;

            Action action = null;
            action = new Action(() => {
                dialogueComplete = true;
                StrangeUtils.RemoveOnceListener(DialogueCompleteSignal, action);
            });
            DialogueCompleteSignal.AddOnce(action);


            string dialogueName = null;
            var combatant = BattleModel.SelectedCombatant;
            if (combatant.Name == "Liat") {
                dialogueName = "liat_inn_visit";
            } else if (combatant.Name == "Janek") {
                dialogueName = "janek_inn_visit";
            } else {
                throw new ArgumentException("Can't find inn visit dialogue for " + combatant.Name);
            }

            var path = Path.Combine("Chapter 2", dialogueName);
            StartDialogueSignal.Dispatch(path);
            while (!dialogueComplete) {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
