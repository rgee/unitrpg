using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Contexts.Global.Services;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using strange.extensions.mediation.impl;
using UnityEngine;

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
        public ISaveGameService SaveGameService { get; set; }

        public override void OnRegister() {
            View.MapClicked.AddListener(OnMapClicked);
            View.MoveComplete.AddListener(OnMoveComplete);
            View.MapHovered.AddListener(OnMapHovered);
            View.FightComplete.AddListener(OnFightComplete);
            FinalizedFightSignal.AddListener(OnFight);

            var map = new Map(View.Width, View.Height);
            foreach (var tile in View.GetObstructedPositions()) {
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
                var config = new MapConfiguration(View.GetDimensions(), View.Map, combatantDb);
                InitializeMapSignal.Dispatch(config);
            });
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