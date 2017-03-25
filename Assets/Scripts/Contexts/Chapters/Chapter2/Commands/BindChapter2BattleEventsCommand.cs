using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Contexts.Chapters.Chapter2.Models;
using Assets.Contexts.Chapters.Chapter2.Signals;
using Assets.Contexts.OverlayDialogue.Signals.Public;
using Contexts.Battle.Models;
using Contexts.Battle.Signals.Public;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2.Commands {
    public class BindChapter2BattleEventsCommand : Command {

        // Event name for spawning reinforcements after maelle is recruited
        private const string MaelleReinforcementsKey = "spawn_maelle_reinforcements";

        // How many turns after Maelle is recruited to spawn the 2nd set of reinforcements
        private const int ReinforcementTurnsAfterMaelle = 2;

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

        [Inject]
        public PushSceneSignal PushSceneSignal { get; set; }

        [Inject]
        public ScenePopCompleteSignal ScenePopCompleteSignal { get; set; }

        [Inject]
        public SpawnCombatantSignal SpawnCombatantSignal { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject View { get; set; }

        public override void Execute() {
            var innHouseDialogues = new Dictionary<string, string> {
                { "Janek", "janek_inn_visit" },
                { "Liat", "liat_inn_visit" }
            };
            EventRegistry.RegisterHandler("inspect_inn", DoVisit(innHouseDialogues, House.Inn));


            var genericHouseDialogues = new Dictionary<string, string> {
                { "Janek", "janek_house_visit" },
                { "Liat", "liat_house_visit" }
            };
            EventRegistry.RegisterHandler("inspect_house", DoVisit(genericHouseDialogues, House.Generic));
            EventRegistry.RegisterHandler(MaelleReinforcementsKey, SpawnMaelleReinforcements());
            EventRegistry.RegisterHandler("inspect_clinic", DoClinicVisit());
            EventRegistry.RegisterHandler("fountain_reinforcements", SpawnFountainReinforcements());
        }

        private IEnumerator SpawnMaelleReinforcements() {
            yield return null;
        }

        private IEnumerator SpawnFountainReinforcements() {
            yield return null;
        }

        private IEnumerator DoClinicVisit() {
            // Wait for the player to go through the dialogue
            var dialogueComplete = false;
            Action action = null;
            action = () => {
                dialogueComplete = true;
                StrangeUtils.RemoveOnceListener(DialogueCompleteSignal, action);
            };
            ScenePopCompleteSignal.AddOnce(action);
            PushSceneSignal.Dispatch("chapter_2_clinic_visit");

            // Preload Maelle in the background.
            var path = "Prefabs/Combatants/Maelle";
            var maellePrefab = Resources.Load(path) as GameObject;
            while (!dialogueComplete) {
                yield return new WaitForEndOfFrame();
            }

            var maelle = GameObject.Instantiate(maellePrefab);

            var units = View.transform.FindChild("Battle View/Map/Units");
            maelle.transform.SetParent(units);

            var position = new Vector2(35, 19);
            var secondPositionOption = new Vector2(37, 19);

            var map = BattleModel.Map;
            if (map.IsBlocked(position)) {
                position = secondPositionOption;
            }

            var dimensions = BattleModel.Dimensions;
            var worldPosition = dimensions.GetWorldPositionForGridPosition(position);
            maelle.transform.position = worldPosition;

            SpawnCombatantSignal.Dispatch(maelle);

            var battle = BattleModel.Battle;
            var reinforcementsTurn = battle.TurnNumber + ReinforcementTurnsAfterMaelle;
            BattleModel.Battle.ScheduleTurnEvent(reinforcementsTurn, MaelleReinforcementsKey);
        }

        private IEnumerator DoVisit(Dictionary<string, string> nameToDialogue, House houseType) {
            // Choose the dialogue based on the current combatant
            var combatant = BattleModel.SelectedCombatant;
            if (!nameToDialogue.ContainsKey(combatant.Name)) {
               throw new ArgumentException("Could not find " + houseType + " visit dialogue for " + combatant.Name); 
            }
            var dialogueName = nameToDialogue[combatant.Name];
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
            yield return new WaitUntil(() => dialogueComplete);

            // Wait for the house light to go off and whatnot.
            var houseDisabled = false;
            Action houseDisabledCallback = null;
            houseDisabledCallback = () => {
                houseDisabled = true;
                StrangeUtils.RemoveOnceListener(HouseLightTransitionCompleteSignal, houseDisabledCallback);
            };
            HouseLightTransitionCompleteSignal.AddListener(houseDisabledCallback);
            MarkHouseVisitedSignal.Dispatch(houseType);
            yield return new WaitUntil(() => houseDisabled);

            Debug.LogFormat("House Visit {0} complete. All done?: {1}", houseType, EastmerePlaza.HouseVisitsComplete);

            // Enable the clinic if the other houses have been visited
            if (EastmerePlaza.HouseVisitsComplete) {

                // Wait for the clinic animation to run 
                Action clinicEnabledCallback = null;
                var clinicEnabled = false;
                clinicEnabledCallback = () => {
                    Debug.Log("Clinic enabled.");
                    clinicEnabled = true;
                    StrangeUtils.RemoveOnceListener(HouseLightTransitionCompleteSignal, clinicEnabledCallback);
                };
                HouseLightTransitionCompleteSignal.AddListener(clinicEnabledCallback);

                Debug.Log("Enabling the clinic.");
                HouseLightEnableSignal.Dispatch(House.Clinic);

                yield return new WaitUntil(() => clinicEnabled);
            }
        }
    }
}
