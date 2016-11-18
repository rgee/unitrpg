using System;
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
        public ActionCompleteSignal ActionCompleteSignal{ get; set; }

        [Inject]
        public NewFinalizedFightSignal FinalizedFightSignal { get; set; }

        [Inject]
        public ContextRequestedSignal ContextRequestedSignal { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public Game Game { get; set; }

        public override void OnRegister() {
            View.MapClicked.AddListener(OnMapClicked);
            View.MoveComplete.AddListener(OnMoveComplete);
            View.MapHovered.AddListener(OnMapHovered);
            View.FightComplete.AddListener(OnFightComplete);
            View.MapRightClicked.AddListener(OnRightClick);
            FinalizedFightSignal.AddListener(OnFight);

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

            MoveCombatantSignal.AddListener(OnMove);
            GatherSignal.AddOnce(() => {
                var config = new MapConfiguration(View.GetDimensions(), View.Map, combatantDb, View.ChapterNumber);
                InitializeMapSignal.Dispatch(config);
            });
        }

        private void OnRightClick(Vector2 position) {
            ContextRequestedSignal.Dispatch(position);
        }

        private void OnMoveComplete() {
           ActionCompleteSignal.Dispatch();
        }

        private void OnFightComplete() {
           ActionCompleteSignal.Dispatch();   
        }

        private void OnMove(MovementPath path) {
            View.MoveUnit(path.Combatant.Id, path.Positions);
        }

        private void OnFight(FinalizedFight finalizedFight) {
            View.AnimateFight(finalizedFight);
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