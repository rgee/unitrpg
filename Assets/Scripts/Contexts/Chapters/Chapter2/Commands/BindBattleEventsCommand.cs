using System;
using System.Collections;
using System.IO;
using Assets.Contexts.Chapters.Chapter2.Models;
using Assets.Contexts.Chapters.Chapter2.Signals;
using Assets.Contexts.OverlayDialogue.Signals.Public;
using Contexts.Battle.Models;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2.Commands {
    public class BindBattleEventsCommand : Command {
        [Inject]
        public StartDialogueSignal StartDialogueSignal { get; set; }

        [Inject]
        public BattleEventRegistry EventRegistry { get; set; }

        [Inject]
        public DialogueCompleteSignal DialogueCompleteSignal { get; set; }

        [Inject]
        public MarkHouseVisitedSignal MarkHouseVisitedSignal { get; set; }

        [Inject]
        public HouseLightTransitionCompleteSignal HouseLightTransitionCompleteSignal { get; set; }

        [Inject]
        public HouseLightEnableSignal HouseLightEnableSignal { get; set; }

        [Inject]
        public BattleViewState BattleModel { get; set; }

        [Inject]
        public EastmerePlazaState EastmerePlaza { get; set; }

        public override void Execute() {
            EventRegistry.RegisterHandler("inspect_inn", ShowVisit());
        }

        private IEnumerator ShowVisit() {

            // Choose the dialogue based on the current combatant
            string dialogueName;
            var combatant = BattleModel.SelectedCombatant;
            if (combatant.Name == "Liat") {
                dialogueName = "liat_inn_visit";
            } else if (combatant.Name == "Janek") {
                dialogueName = "janek_inn_visit";
            } else {
                throw new ArgumentException("Can't find inn visit dialogue for " + combatant.Name);
            }
            var path = Path.Combine("Chapter 2", dialogueName);

            // Wait for the player to go through the dialogue
            var dialogueComplete = false;
            Action action = null;
            action = () => {
                dialogueComplete = true;
                StrangeUtils.RemoveOnceListener(DialogueCompleteSignal, action);
            };
            DialogueCompleteSignal.AddOnce(action);
            StartDialogueSignal.Dispatch(path);
            while (!dialogueComplete) {
                yield return new WaitForEndOfFrame();
            }

            // Wait for the house light to go off and whatnot.
            var houseDisabled = false;
            Action houseDisabledCallback = null;
            houseDisabledCallback = () => {
                houseDisabled = true;
                StrangeUtils.RemoveOnceListener(HouseLightTransitionCompleteSignal, houseDisabledCallback);
            };
            HouseLightTransitionCompleteSignal.AddListener(houseDisabledCallback);
            MarkHouseVisitedSignal.Dispatch(House.Inn);
            while (!houseDisabled) {
                yield return new WaitForEndOfFrame();
            }

            if (EastmerePlaza.HouseVisitsComplete) {
               HouseLightEnableSignal.Dispatch(House.Clinic); 
            }
        }
    }
}
