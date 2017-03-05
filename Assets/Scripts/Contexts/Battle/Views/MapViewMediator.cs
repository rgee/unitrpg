using System;
using System.Collections;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Contexts.Global.Models;
using Contexts.Global.Services;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using strange.extensions.mediation.impl;
using UnityEngine;
using MapConfiguration = Contexts.Battle.Utilities.MapConfiguration;

namespace Contexts.Battle.Views {
    public class MapViewMediator : Mediator {
        [Inject]
        public MapView View { get; set; }

        [Inject]
        public BattleViewState BattleModel { get; set; }

        [Inject]
        public MapPositionClickedSignal MapPositionClickedSignal { get; set; }

        [Inject]
        public InitializeMapSignal InitializeMapSignal { get; set; }

        [Inject]
        public GatherBattleFromEditorSignal GatherSignal { get; set; }

        [Inject]
        public HoverPositionSignal HoverPositionSignal { get; set; }

        [Inject]
        public MoveCombatantSignal MoveCombatantSignal { get; set; }

        [Inject]
        public ActionAnimationCompleteSignal ActionAnimationCompleteSignal{ get; set; }

        [Inject]
        public ContextRequestedSignal ContextRequestedSignal { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public AnimateActionSignal AnimateActionSignal { get; set; }

        [Inject]
        public Game Game { get; set; }

        public override void OnRegister() {
            View.MapClicked.AddListener(OnMapClicked);
            View.MapHovered.AddListener(OnMapHovered);
            View.MapRightClicked.AddListener(OnRightClick);
            AnimateActionSignal.AddListener(AnimateAction);

            if (!Game.Maps.ContainsKey(View.MapId)) {
                throw new Exception("Could not find map by id " + View.MapId);
            }

            var mapConfig = Game.Maps[View.MapId];
            var map = new Map(mapConfig.Width, mapConfig.Height);
            foreach (var tile in mapConfig.ObstructionLocations) {
                map.AddObstruction(tile);
            }

            var combatantReferences = View.GetCombatants();
            var combatantDb = new CombatantDatabase(combatantReferences, SaveGameService.CurrentSave);
            foreach (var combatant in combatantDb.GetAllCombatants()) {
                map.AddCombatant(combatant);
            }

            View.CombatantDatabase = combatantDb;
            View.Map = map;

            GatherSignal.AddOnce(() => {
                var config = new MapConfiguration(View.GetDimensions(), View.Map, combatantDb, View.ChapterNumber, View.MapId);
                InitializeMapSignal.Dispatch(config);
            });
        }

        private void AnimateAction(ICombatAction action) {
            if (action is MoveAction) {
                var move = action as MoveAction;
                StartCoroutine(AnimateMove(move));
            } else if (action is FightAction) {
                var fight = action as FightAction;
                StartCoroutine(AnimateFight(fight));
            }
        }

        private IEnumerator AnimateFight(FightAction action) {
            yield return StartCoroutine(View.AnimateFight(action.Fight));
            ActionAnimationCompleteSignal.Dispatch(action);
        }

        private IEnumerator AnimateMove(MoveAction action) {
            var combatant = action.Combatant;
            yield return StartCoroutine(View.MoveUnit(combatant.Id, action.Path));
            ActionAnimationCompleteSignal.Dispatch(action);
        }

        private void OnRightClick(Vector2 position) {
            ContextRequestedSignal.Dispatch(position);
        }

        private void OnMapHovered(Vector2 hoverPosition) {
            var worldPosition = View.GetDimensions().GetWorldPositionForGridPosition(hoverPosition);
            HoverPositionSignal.Dispatch(new GridPosition(hoverPosition, worldPosition));
        }

        private void OnMapClicked(Vector2 clickPosition) {
            MapPositionClickedSignal.Dispatch(clickPosition);
        }
    }
}