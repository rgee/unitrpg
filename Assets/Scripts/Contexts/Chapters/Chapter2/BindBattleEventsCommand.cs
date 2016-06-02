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

            var path = Path.Combine("Chapter 2", "liat_inn_visit");
            StartDialogueSignal.Dispatch(path);
            while (!dialogueComplete) {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
